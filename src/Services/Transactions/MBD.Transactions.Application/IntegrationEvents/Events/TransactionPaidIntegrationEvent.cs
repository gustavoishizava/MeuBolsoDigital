using System;
using MBD.Transactions.Domain.Enumerations;

namespace MBD.Transactions.Application.IntegrationEvents.Events
{
    public class TransactionPaidIntegrationEvent
    {
        public Guid Id { get; private init; }
        public Guid BankAccountId { get; private init; }
        public string Type { get; private init; }
        public decimal Value { get; private init; }
        public DateTime Date { get; private init; }

        public TransactionPaidIntegrationEvent(Guid id, decimal value, DateTime date, Guid bankAccountId, TransactionType type)
        {
            Id = id;
            Value = value;
            Date = date;
            BankAccountId = bankAccountId;
            Type = type.ToString();
        }
    }
}