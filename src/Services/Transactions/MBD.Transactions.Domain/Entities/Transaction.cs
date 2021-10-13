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
        public Guid? CreditCardBillId { get; private set; }
        public DateTime ReferenceDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public TransactionStatus Status { get; private set; }
        public decimal Value { get; private set; }
        public string Description { get; private set; }

        public bool ItsPaid => PaymentDate != null && Status == TransactionStatus.Paid;
        public Category Category { get; private set; }
        private bool _valueChanged { get; set; } = false;

        public Transaction(Guid tenantId, BankAccount bankAccount, Category category, DateTime referenceDate, DateTime dueDate, decimal value, string description)
        {
            Assertions.IsGreaterOrEqualsThan(value, 0, "O valor não pode ser menor que 0.");

            TenantId = tenantId;
            BankAccountId = bankAccount.Id;
            CategoryId = category.Id;
            Category = category;
            CreditCardBillId = null;
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
            if (!ItsPaid || PaymentDate != paymentDate || _valueChanged)
                AddDomainEvent(new RealizedPaymentDomainEvent(Id, paymentDate, Value, BankAccountId, Category.Type));

            PaymentDate = paymentDate;
            Status = TransactionStatus.Paid;
        }

        public void UndoPayment()
        {
            PaymentDate = null;
            Status = TransactionStatus.AwaitingPayment;

            AddDomainEvent(new ReversedPaymentDomainEvent(Id));
        }

        public void SetValue(decimal value)
        {
            Assertions.IsGreaterOrEqualsThan(value, 0, "O valor não pode ser menor que 0.");

            _valueChanged = value != Value;
            if (_valueChanged)
                AddDomainEvent(new ValueChangedDomainEvent(Id, Value, value));

            Value = value;
        }

        public void Update(BankAccount bankAccount, Category category, DateTime referenceDate, DateTime dueDate, decimal value, string description)
        {
            BankAccountId = bankAccount.Id;
            CategoryId = category.Id;
            ReferenceDate = referenceDate;
            DueDate = dueDate;
            SetValue(value);
            Description = description;

            AddDomainEvent(new TransactionUpdatedDomainEvent(this, bankAccount, category));
        }

        public void LinkCreditCardBill(Guid creditCardBillId)
        {
            Assertions.IsNull(CreditCardBillId, "A transação já possui fatura de cartão de crédito vinculada.");
            Assertions.IsNotEmpty(creditCardBillId, "O Id da fatura do cartão de crédito está inválido.");
            Assertions.IsFalse(ItsPaid, "Não é possível vincular uma fatura de cartão de crédito a uma transação já paga.");
            Assertions.IsTrue(Category.Type == TransactionType.Expense, "Não é possível vincular uma fatura de cartão de crédito a uma transação de receita.");

            CreditCardBillId = creditCardBillId;
            AddDomainEvent(new LinkedToCreditCardBillDomainEvent(Id, BankAccountId, CreditCardBillId.Value, CreatedAt, Value));
        }

        public void UnlinkCreditCardBill()
        {
            if (CreditCardBillId is null)
                return;

            AddDomainEvent(new UnlinkedToCreditCardBillDomainEvent(Id, BankAccountId, CreditCardBillId.Value));
            CreditCardBillId = null;
        }
    }
}