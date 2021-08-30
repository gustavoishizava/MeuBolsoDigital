using System;
using MediatR;

namespace MBD.Transactions.Application.IntegrationEvents.Events
{
    public class TransactionPaidIntegrationEvent : INotification
    {
        public Guid Id { get; private init; }
        public decimal Value { get; private init; }
        public DateTime Date { get; private init; }

        public TransactionPaidIntegrationEvent(Guid id, decimal value, DateTime date)
        {
            Id = id;
            Value = value;
            Date = date;
        }
    }
}