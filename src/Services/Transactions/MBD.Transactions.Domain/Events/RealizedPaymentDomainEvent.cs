using System;
using MBD.Transactions.Domain.Events.Common;

namespace MBD.Transactions.Domain.Events
{
    public class RealizedPaymentDomainEvent : DomainEvent
    {
        public Guid Id { get; private init; }
        public DateTime Date { get; private init; }
        public decimal Value { get; private init; }

        public RealizedPaymentDomainEvent(Guid id, DateTime date, decimal value)
        {
            AggregateId = id;
            Id = id;
            Date = date;
            Value = value;
        }
    }
}