using System;

namespace MBD.IntegrationEventLog
{
    public class IntegrationEventLogEntry
    {
        public Guid Id { get; private init; }
        public string EventTypeName { get; private init; }
        public string Content { get; private init; }

        public IntegrationEventLogEntry(string eventTypeName, string content)
        {
            Id = Guid.NewGuid();
            EventTypeName = eventTypeName ?? throw new ArgumentNullException(nameof(eventTypeName));
            Content = content ?? throw new ArgumentNullException(nameof(eventTypeName));
        }
    }
}