using System;

namespace MBD.Transactions.Application.IntegrationEvents.Events
{
    public class TransactionUndoPaymentIntegrationEvent
    {
        public Guid Id { get; private init; }

        public TransactionUndoPaymentIntegrationEvent(Guid id)
        {
            Id = id;
        }
    }
}