using System.Text;
using System;
using System.Text.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MBD.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly RabbitMqConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        public bool IsConnected => _connection?.IsOpen ?? false;
        private readonly ILogger<MessageBus> _logger;

        public MessageBus(IOptions<RabbitMqConfiguration> options, ILogger<MessageBus> logger)
        {
            _configuration = options.Value;
            _logger = logger;
        }

        public void Publish<T>(T message) where T : class
        {
            TryConnect();

            _channel.QueueDeclare(
                queue: message.GetType().Name,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var stringfiedMessage = JsonSerializer.Serialize(message, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            var messageBytes = Encoding.UTF8.GetBytes(stringfiedMessage);

            _channel.BasicPublish(
                exchange: string.Empty,
                routingKey: message.GetType().Name,
                basicProperties: null,
                body: messageBytes);

            _logger.LogInformation($"Mensagem publicada: {stringfiedMessage}");
        }

        public void Publish<T>(T message, string queueName) where T : class
        {
            TryConnect();

            _channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var stringfiedMessage = JsonSerializer.Serialize(message, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            var messageBytes = Encoding.UTF8.GetBytes(stringfiedMessage);

            _channel.BasicPublish(
                exchange: string.Empty,
                routingKey: queueName,
                basicProperties: null,
                body: messageBytes);

            _logger.LogInformation($"Mensagem publicada: {stringfiedMessage}");
        }

        public void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class
        {
            TryConnect();

            _logger.LogInformation("Inscrito na fila.");

            _channel.QueueDeclare(
                queue: subscriptionId,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(_channel);

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
                    _logger.LogInformation($"Processando mensagem: {contentString}");

                    onMessage(message);
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e.Message);
                    throw;
                }
            };

            _channel.BasicConsume(
                queue: subscriptionId,
                autoAck: false,
                consumer: consumer);
        }

        public void SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class
        {
            TryConnect();

            _channel.QueueDeclare(
                queue: subscriptionId,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonSerializer.Deserialize<T>(contentString, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                try
                {
                    _logger.LogInformation($"Processando mensagem: {contentString}");

                    await onMessage(message);
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e.Message);
                    throw;
                }
            };

            _channel.BasicConsume(
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
                _logger.LogInformation("Iniciando conexão ao RabbitMQ.");

                var factory = new ConnectionFactory()
                {
                    HostName = _configuration.HostName,
                    UserName = _configuration.UserName,
                    Password = _configuration.Password
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _connection.ConnectionShutdown += OnDisconnect;

                _logger.LogInformation("Conectado com sucesso ao RabbitMQ.");
            });
        }

        private void OnDisconnect(object s, EventArgs e)
        {
            _logger.LogInformation("Desconectando do RabbitMQ.");

            var policy = Policy.Handle<RabbitMQClientException>()
               .Or<BrokerUnreachableException>()
               .RetryForever();

            policy.Execute(() =>
            {
                _logger.LogInformation("Tentanto reconexão ao RabbitMQ.");
                TryConnect();
            });
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}