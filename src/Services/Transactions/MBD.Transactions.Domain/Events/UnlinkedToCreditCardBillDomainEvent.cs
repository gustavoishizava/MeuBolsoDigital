using System;
using MBD.Transactions.Domain.Events.Common;

namespace MBD.Transactions.Domain.Events
{
    public class UnlinkedToCreditCardBillDomainEvent : DomainEvent
    {
        public Guid Id { get; private set; }
        public Guid BankAccountId { get; private init; }
        public Guid CreditCardBillId { get; private init; }

        public UnlinkedToCreditCardBillDomainEvent(Guid id, Guid bankAccountId, Guid creditCardBillId)
        {
            AggregateId = id;
            Id = id;
            BankAccountId = bankAccountId;
            CreditCardBillId = creditCardBillId;
        }
    }
}