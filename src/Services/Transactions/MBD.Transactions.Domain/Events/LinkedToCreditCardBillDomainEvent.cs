using System;
using MBD.Transactions.Domain.Events.Common;

namespace MBD.Transactions.Domain.Events
{
    public class LinkedToCreditCardBillDomainEvent : DomainEvent
    {
        public Guid Id { get; private init; }
        public Guid BankAccountId { get; private init; }
        public Guid CreditCardBillId { get; private init; }
        public DateTime CreatedAt { get; private init; }
        public decimal Value { get; private init; }

        public LinkedToCreditCardBillDomainEvent(Guid id, Guid bankAccountId, Guid creditCardBillId, DateTime createdAt, decimal value)
        {
            AggregateId = id;
            Id = id;
            CreatedAt = createdAt;
            Value = value;
            BankAccountId = bankAccountId;
            CreditCardBillId = creditCardBillId;
        }
    }
}