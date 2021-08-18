using System;
using System.Threading.Tasks;

namespace MessageBus
{
    public interface IMessageBus : IDisposable
    {
        bool IsConnected { get; }

        void Publish<T>(T message) where T : class;
        Task PublishAsync<T>(T message) where T : class;
        void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class;
        Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class;
    }
}