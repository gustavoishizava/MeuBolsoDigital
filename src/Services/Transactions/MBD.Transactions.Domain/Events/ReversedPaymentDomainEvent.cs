using System;
using MBD.Transactions.Domain.Events.Common;

namespace MBD.Transactions.Domain.Events
{
    public class ReversedPaymentDomainEvent : DomainEvent
    {
        public Guid Id { get; private init; }
        public decimal Value { get; private init; }

        public ReversedPaymentDomainEvent(Guid id, decimal value)
        {
            AggregateId = id;
            Id = id;
            Value = value;
        }
    }
}