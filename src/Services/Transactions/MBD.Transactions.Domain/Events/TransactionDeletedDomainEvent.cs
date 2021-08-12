using System;
using MBD.Transactions.Domain.Events.Common;

namespace MBD.Transactions.Domain.Events
{
    public class TransactionDeletedDomainEvent : DomainEvent
    {
        public Guid TransactionId { get; private init; }
        public TransactionDeletedDomainEvent(Guid transactionId)
        {
            AggregateId = transactionId;
            TransactionId = transactionId;
        }
    }
}