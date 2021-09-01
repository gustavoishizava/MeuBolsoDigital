using System;
using MBD.Core;
using MBD.Core.Entities;
using MBD.Transactions.Domain.Entities.Common;
using MBD.Transactions.Domain.Enumerations;
using MBD.Transactions.Domain.Events;
using MBD.Transactions.Domain.ValueObjects;

namespace MBD.Transactions.Domain.Entities
{
    public class Transaction : BaseEntityWithEvent, IAggregateRoot
    {
        public Guid TenantId { get; private init; }
        public Guid BankAccountId { get; private set; }
        public Guid CategoryId { get; private set; }
        public DateTime ReferenceDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public TransactionStatus Status { get; private set; }
        public decimal Value { get; private set; }
        public string Description { get; private set; }

        public bool ItsPaid => PaymentDate != null && Status == TransactionStatus.Paid;
        public Category Category { get; private set; }

        public Transaction(Guid tenantId, BankAccount bankAccount, Category category, DateTime referenceDate, DateTime dueDate, decimal value, string description)
        {
            Assertions.IsGreaterOrEqualsThan(value, 0, "O valor não pode ser menor que 0.");

            TenantId = tenantId;
            BankAccountId = bankAccount.Id;
            CategoryId = category.Id;
            Category = category;
            ReferenceDate = referenceDate;
            DueDate = dueDate;
            PaymentDate = null;
            Status = TransactionStatus.AwaitingPayment;
            Value = value;
            Description = description;

            AddDomainEvent(new TransactionCreatedDomainEvent(this, bankAccount.Description, category.Name));
        }

        #region EF
        protected Transaction() { }
        #endregion

        public void Pay(DateTime paymentDate)
        {
            PaymentDate = paymentDate;
            Status = TransactionStatus.Paid;

            AddDomainEvent(new RealizedPaymentDomainEvent(Id, PaymentDate.Value, Value, BankAccountId, Category.Type));
        }

        public void UndoPayment()
        {
            PaymentDate = null;
            Status = TransactionStatus.AwaitingPayment;

            AddDomainEvent(new ReversedPaymentDomainEvent(Id, Value));
        }

        public void SetValue(decimal value)
        {
            Assertions.IsGreaterOrEqualsThan(value, 0, "O valor não pode ser menor que 0.");
            AddDomainEvent(new ValueChangedDomainEvent(Id, Value, value));

            Value = value;
        }

        public void Update(BankAccount bankAccount, Category category, DateTime referenceDate, DateTime dueDate, decimal value, string description)
        {
            BankAccountId = bankAccount.Id;
            CategoryId = category.Id;
            ReferenceDate = referenceDate;
            DueDate = dueDate;
            Status = TransactionStatus.AwaitingPayment;
            SetValue(value);
            Description = description;

            AddDomainEvent(new TransactionUpdatedDomainEvent(this, bankAccount, category));
        }
    }
}