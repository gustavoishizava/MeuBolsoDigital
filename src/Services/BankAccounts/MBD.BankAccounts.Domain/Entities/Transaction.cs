using System;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.Core;

namespace MBD.BankAccounts.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; private init; }
        public Guid AccountId { get; private init; }
        public DateTime CreatedAt { get; private set; }
        public decimal Value { get; private set; }
        public TransactionType Type { get; private init; }

        internal Transaction(Guid id, Guid accountId, DateTime createdAt, decimal value, TransactionType type)
        {
            Id = id;
            AccountId = accountId;
            CreatedAt = createdAt;
            SetValue(value);
            Type = type;
        }

        internal void SetValue(decimal value)
        {
            Assertions.IsGreaterOrEqualsThan(value, 0, "O valor não pode ser menor que 0.");
            Value = value;
        }

        internal void SetDate(DateTime date)
        {
            CreatedAt = date;
        }
    }
}