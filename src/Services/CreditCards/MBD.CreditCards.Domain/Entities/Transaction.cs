using System;
using MBD.Core;

namespace MBD.CreditCards.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public Guid CreditCardBillId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public decimal Value { get; private set; }

        internal Transaction(Guid id, Guid creditCardBillId, decimal value, DateTime createdAt)
        {
            Assertions.IsGreaterOrEqualsThan(value, 0, "O valor não pode ser menor que R$0,00.");

            Id = id;
            CreditCardBillId = creditCardBillId;
            CreatedAt = createdAt;
            Value = value;
        }
    }
}