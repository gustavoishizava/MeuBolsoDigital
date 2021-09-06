using System;
using MediatR;

namespace MBD.Transactions.Application.IntegrationEvents.Events
{
    public class TransactionUndoPaymentIntegrationEvent : INotification
    {
        public Guid Id { get; private init; }

        public TransactionUndoPaymentIntegrationEvent(Guid id)
        {
            Id = id;
        }
    }
}