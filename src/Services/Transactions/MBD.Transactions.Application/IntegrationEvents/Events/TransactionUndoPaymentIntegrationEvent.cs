using System;
using MediatR;

namespace MBD.Transactions.Application.IntegrationEvents.Events
{
    public class TransactionUndoPaymentIntegrationEvent : INotification
    {
        public Guid Id { get; private init; }
        public decimal Value { get; private init; }

        public TransactionUndoPaymentIntegrationEvent(Guid id, decimal value)
        {
            Id = id;
            Value = value;
        }
    }
}