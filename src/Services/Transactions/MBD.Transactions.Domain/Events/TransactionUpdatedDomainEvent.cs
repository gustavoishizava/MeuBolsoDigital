using System;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Enumerations;
using MBD.Transactions.Domain.Events.Common;
using MBD.Transactions.Domain.ValueObjects;

namespace MBD.Transactions.Domain.Events
{
    public class TransactionUpdatedDomainEvent : DomainEvent
    {
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

        public TransactionUpdatedDomainEvent(Transaction transaction, BankAccount bankAccount, Category category)
        {
            AggregateId = transaction.Id;
            BankAccountId = bankAccount.Id;
            BankAccountDescription = bankAccount.Description;
            CategoryId = category.Id;
            CategoryName = category.Name;
            ReferenceDate = transaction.ReferenceDate;
            DueDate = transaction.DueDate;
            PaymentDate = transaction.PaymentDate;
            Status = transaction.Status;
            Value = transaction.Value;
            Description = transaction.Description;
        }
    }
}