using System.Text;
using System;
using System.Text.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;

namespace MBD.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly RabbitMqConfiguration _configuration;
        private IConnection _connection;
        public bool IsConnected => _connection?.IsOpen ?? false;

        public MessageBus(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;
        }

        public void Publish<T>(T message) where T : class
        {
            TryConnect();

            using var channel = _connection.CreateModel();

            channel.QueueDeclare(
                queue: nameof(T),
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var stringfiedMessage = JsonSerializer.Serialize(message, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            var messageBytes = Encoding.UTF8.GetBytes(stringfiedMessage);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: nameof(T),
                basicProperties: null,
                body: messageBytes);
        }

        public void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class
        {
            using var channel = _connection.CreateModel();

            channel.QueueDeclare(
                queue: subscriptionId,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonSerializer.Deserialize<T>(contentString, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                try
                {
                    onMessage(message);
                }
                catch
                {
                    channel.BasicAck(eventArgs.DeliveryTag, false);
                    throw;
                }
            };

            channel.BasicConsume(
                queue: subscriptionId,
                autoAck: false,
                consumer: consumer);
        }

        private void TryConnect()
        {
            if (IsConnected)
                return;

            var policy = Policy.Handle<ConnectFailureException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(3, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)));

            policy.Execute(() =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _configuration.HostName,
                    UserName = _configuration.UserName,
                    Password = _configuration.Password
                };
                _connection = factory.CreateConnection();
                _connection.ConnectionShutdown += OnDisconnect;
            });
        }

        private void OnDisconnect(object s, EventArgs e)
        {
            var policy = Policy.Handle<RabbitMQClientException>()
               .Or<BrokerUnreachableException>()
               .RetryForever();

            policy.Execute(() =>
            {
                TryConnect();
            });
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}