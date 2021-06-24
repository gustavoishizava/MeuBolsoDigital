using System;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.Core;

namespace MBD.BankAccounts.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public Guid AccountId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public decimal Value { get; private set; }
        public TransactionType Type { get; private set; }

        internal Transaction(Guid id, Guid accountId, DateTime createdAt, decimal value, TransactionType type)
        {
            Assertions.IsGreaterOrEqualsThan(value, 0, "O valor não pode ser menor que 0.");

            Id = id;
            AccountId = accountId;
            CreatedAt = createdAt;
            Value = value;
            Type = type;
        }
    }
}