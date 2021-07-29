using System;
using MBD.Core;
using MBD.Core.Entities;
using MBD.Transactions.Domain.Enumerations;

namespace MBD.Transactions.Domain.Entities
{
    public class Transaction : BaseEntity, IAggregateRoot
    {
        public Guid TenantId { get; private set; }
        public Guid BankAccountId { get; private set; }
        public Guid CategoryId { get; private set; }
        public DateTime ReferenceDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public TransactionStatus Status { get; private set; }
        public decimal Value { get; private set; }
        public string Description { get; private set; }

        public bool ItsPaid => PaymentDate != null && Status == TransactionStatus.Paid;

        public Transaction(Guid tenantId, Guid bankAccountId, Guid categoryId, DateTime referenceDate, DateTime dueDate, decimal value, string description)
        {
            TenantId = tenantId;
            BankAccountId = bankAccountId;
            CategoryId = categoryId;
            ReferenceDate = referenceDate;
            DueDate = dueDate;
            PaymentDate = null;
            Status = TransactionStatus.AwaitingPayment;
            SetValue(value);
            Description = description;
        }

        public void Pay(DateTime paymentDate)
        {
            PaymentDate = paymentDate;
            Status = TransactionStatus.Paid;
        }

        public void UndoPayment()
        {
            PaymentDate = null;
            Status = TransactionStatus.AwaitingPayment;
        }

        public void SetValue(decimal value)
        {
            Assertions.IsGreaterOrEqualsThan(value, 0, "O valor não pode ser menor que 0.");

            Value = value;
        }
    }
}