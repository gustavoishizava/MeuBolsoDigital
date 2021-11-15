using System;

namespace MBD.CreditCards.Domain.Entities
{
    public class BankAccount
    {
        public Guid Id { get; private init; }
        public string Description { get; private init; }
    }
}