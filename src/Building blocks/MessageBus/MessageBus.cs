using System;
using System.Threading.Tasks;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace MessageBus
{
    public class MessageBus : IMessageBus
    {
        private IConnection _connection;

        public bool IsConnected => _connection?.IsOpen ?? false;

        public void Publish<T>(T message) where T : class
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync<T>(T message) where T : class
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class
        {
            throw new NotImplementedException();
        }

        public Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class
        {
            throw new NotImplementedException();
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
                    HostName = "localhost"
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