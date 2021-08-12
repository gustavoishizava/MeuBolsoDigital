using System;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Enumerations;
using MBD.Transactions.Domain.Events.Common;

namespace MBD.Transactions.Domain.Events
{
    public class TransactionCreatedDomainEvent : DomainEvent
    {
        public Guid TenantId { get; private init; }
        public Guid BankAccountId { get; private init; }
        public string BankAccountDescription { get; private init; }
        public Guid CategoryId { get; private init; }
        public string CategoryName { get; private init; }
        public DateTime ReferenceDate { get; private init; }
        public DateTime DueDate { get; private init; }
        public DateTime? PaymentDate { get; private init; }
        public TransactionStatus Status { get; private init; }
        public decimal Value { get; private init; }
        public string Description { get; private init; }

        public TransactionCreatedDomainEvent(Transaction transaction, string bankAccountDescription, string categoryName)
        {
            AggregateId = transaction.Id;
            TenantId = transaction.TenantId;
            BankAccountId = transaction.BankAccountId;
            BankAccountDescription = bankAccountDescription;
            CategoryId = transaction.CategoryId;
            CategoryName = categoryName;
            ReferenceDate = transaction.ReferenceDate;
            DueDate = transaction.DueDate;
            PaymentDate = transaction.PaymentDate;
            Status = transaction.Status;
            Value = transaction.Value;
            Description = transaction.Description;
        }
    }
}