using System;

namespace MBD.Transactions.Domain.ValueObjects
{
    public class BankAccount
    {
        public Guid Id { get; init; }
        public string Description { get; init; }
    }
}