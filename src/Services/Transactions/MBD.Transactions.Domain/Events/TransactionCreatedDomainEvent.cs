using System;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Enumerations;
using MBD.Transactions.Domain.Events.Common;

namespace MBD.Transactions.Domain.Events
{
    public class TransactionCreatedDomainEvent : DomainEvent
    {
        public Guid TenantId { get; private set; }
        public Guid BankAccountId { get; private set; }
        public string BankAccountDescription { get; private set; }
        public Guid CategoryId { get; private set; }
        public string CategoryName { get; private set; }
        public DateTime ReferenceDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public TransactionStatus Status { get; private set; }
        public decimal Value { get; private set; }
        public string Description { get; private set; }

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