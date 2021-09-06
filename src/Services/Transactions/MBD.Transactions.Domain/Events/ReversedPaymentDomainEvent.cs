using System;
using MBD.Transactions.Domain.Events.Common;

namespace MBD.Transactions.Domain.Events
{
    public class ReversedPaymentDomainEvent : DomainEvent
    {
        public Guid Id { get; private init; }

        public ReversedPaymentDomainEvent(Guid id)
        {
            AggregateId = id;
            Id = id;
        }
    }
}