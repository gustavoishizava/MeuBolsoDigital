using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace MBD.MessageBus
{
    public interface IMessageBus : IDisposable
    {
        IModel Channel { get; }
        bool IsConnected { get; }

        void Publish<T>(T message) where T : class;
        void Publish<T>(T message, string queueName) where T : class;
        void Publish<T>(T message, string queueName, string exchange) where T : class;
        void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class;
        void SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class;
    }
}