using System;
using MBD.Transactions.Domain.Events.Common;

namespace MBD.Transactions.Domain.Events
{
    public class ValueChangedDomainEvent : DomainEvent
    {
        public Guid Id { get; private init; }
        public decimal OldValue { get; private init; }
        public decimal NewValue { get; private init; }

        public ValueChangedDomainEvent(Guid id, decimal oldValue, decimal newValue)
        {
            Id = id;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}