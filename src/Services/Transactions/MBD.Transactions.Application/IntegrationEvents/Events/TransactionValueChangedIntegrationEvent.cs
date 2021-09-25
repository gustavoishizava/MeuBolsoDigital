using System;

namespace MBD.Transactions.Application.IntegrationEvents.Events
{
    public class TransactionValueChangedIntegrationEvent
    {
        public Guid Id { get; init; }
        public decimal NewValue { get; init; }
        public decimal OldValue { get; init; }

        public TransactionValueChangedIntegrationEvent(Guid id, decimal newValue, decimal oldValue)
        {
            Id = id;
            NewValue = newValue;
            OldValue = oldValue;
        }
    }
}