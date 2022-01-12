using System;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Enumerations;
using MBD.Transactions.Domain.Events.Common;

namespace MBD.Transactions.Domain.Events
{
    public class TransactionCreatedDomainEvent : DomainEvent
    {
        public Guid TenantId { get; private init; }
        public BankAccount BankAccount { get; private init; }
        public Category Category { get; private init; }
        public DateTime ReferenceDate { get; private init; }
        public DateTime DueDate { get; private init; }
        public DateTime? PaymentDate { get; private init; }
        public TransactionStatus Status { get; private init; }
        public decimal Value { get; private init; }
        public string Description { get; private init; }

        public TransactionCreatedDomainEvent(Transaction transaction)
        {
            AggregateId = transaction.Id;
            TenantId = transaction.TenantId;
            BankAccount = transaction.BankAccount;
            Category = transaction.Category;
            ReferenceDate = transaction.ReferenceDate;
            DueDate = transaction.DueDate;
            PaymentDate = transaction.PaymentDate;
            Status = transaction.Status;
            Value = transaction.Value;
            Description = transaction.Description;
        }
    }
}