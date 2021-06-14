using System;
using MBD.Core.Entities;
using MBD.Transactions.Domain.Enumerations;

namespace MBD.Transactions.Domain.Entities
{
    public class Transaction : BaseEntity, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public Guid BankAccountId { get; private set; }
        public Guid CategoryId { get; private set; }
        public DateTime ReferenceDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public TransactionStatus Status { get; private set; }
        public decimal Value { get; private set; }
        public string Description { get; private set; }

        public Category Category { get; private set; }
    }
}