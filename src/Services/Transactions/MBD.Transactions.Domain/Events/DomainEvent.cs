using System;

namespace MBD.Transactions.Domain.Events
{
    public abstract class DomainEvent
    {
        public Guid AggregateId { get; protected set; }
        public DateTime TimeStamp { get; private set; }

        public DomainEvent()
        {
            TimeStamp = DateTime.Now;
        }
    }
}