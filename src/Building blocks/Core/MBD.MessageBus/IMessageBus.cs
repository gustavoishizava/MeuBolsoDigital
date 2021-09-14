using System;

namespace MBD.MessageBus
{
    public interface IMessageBus : IDisposable
    {
        bool IsConnected { get; }

        void Publish<T>(T message) where T : class;
        void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class;
    }
}