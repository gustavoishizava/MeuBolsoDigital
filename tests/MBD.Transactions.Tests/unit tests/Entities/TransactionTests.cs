using System;
using System.Linq;
using MBD.Core.DomainObjects;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Enumerations;
using MBD.Transactions.Domain.ValueObjects;
using Xunit;

namespace MBD.Transactions.Tests.unit_tests.Entities
{
    public class TransactionTests
    {
        private readonly Transaction _validTransaction;
        private readonly BankAccount _bankAccount;
        private readonly Category _category;

        public TransactionTests()
        {
            _bankAccount = new BankAccount { Id = Guid.NewGuid(), Description = "Nubank" };
            _category = new Category(Guid.NewGuid(), "Category", TransactionType.Income);
            _validTransaction = new Transaction(Guid.NewGuid(), _bankAccount, _category, DateTime.Now, DateTime.Now.AddDays(5), 100, string.Empty);
        }

        [Theory(DisplayName = "Criar transação com valor inválido deve retornar Domain Exception.")]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        public void InvalidValue_NewTransaction_ReturnDomainException(decimal value)
        {
            // Arrange && Act && Assert
            Assert.Throws<DomainException>(() => new Transaction(Guid.NewGuid(), _bankAccount, _category, DateTime.Now, DateTime.Now, value, string.Empty));
        }

        [Theory(DisplayName = "Criar transação com parâmetros válidos deve retornar sucesso.")]
        [InlineData(0, "Zero")]
        [InlineData(10, "Dez")]
        [InlineData(100.50, "Cem")]
        [InlineData(1000, "Mil")]
        [InlineData(10000.50, "Dez mil")]
        public void ValidParameters_NewTransaction_ReturnSuccess(decimal value, string description)
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var referenceDate = DateTime.Now;
            var dueDate = referenceDate.AddDays(5);

            // Act
            var transaction = new Transaction(tenantId, _bankAccount, _category, referenceDate, dueDate, value, description);

            // Assert
            Assert.Equal(tenantId, transaction.TenantId);
            Assert.Equal(_bankAccount.Id, transaction.BankAccountId);
            Assert.Equal(_category.Id, transaction.CategoryId);
            Assert.Equal(_category, transaction.Category);
            Assert.Equal(referenceDate, transaction.ReferenceDate);
            Assert.Equal(dueDate, transaction.DueDate);
            Assert.Equal(TransactionStatus.AwaitingPayment, transaction.Status);
            Assert.Equal(description, transaction.Description);
            Assert.Equal(value, transaction.Value);
            Assert.Null(transaction.PaymentDate);
            Assert.Null(transaction.CreditCardBillId);
            Assert.Single(transaction.Events);
        }

        [Fact(DisplayName = "Efetuar o pagamento de uma transação válida deve retornar sucesso.")]
        public void ValidTransaction_ProcessPayment_ReturnSuccess()
        {
            // Arrange
            var paymentDate = _validTransaction.DueDate;
            _validTransaction.ClearDomainEvents();

            // Act
            _validTransaction.Pay(paymentDate);

            // Assert
            Assert.Single(_validTransaction.Events);
            Assert.True(_validTransaction.ItsPaid);
            Assert.NotNull(_validTransaction.PaymentDate);
            Assert.Equal(paymentDate, _validTransaction.PaymentDate);
            Assert.Equal(TransactionStatus.Paid, _validTransaction.Status);
        }

        [Fact(DisplayName = "Desfazer o pagamento de uma transação paga.")]
        public void PaidTransaction_UndoPayment_ReturnSuccess()
        {
            // Arrange
            _validTransaction.Pay(_validTransaction.DueDate);
            var currentStatus = _validTransaction.Status;
            var currentPaymentDate = _validTransaction.PaymentDate;
            _validTransaction.ClearDomainEvents();

            // Act
            _validTransaction.UndoPayment();

            // Assert
            Assert.Single(_validTransaction.Events);
            Assert.Null(_validTransaction.PaymentDate);
            Assert.NotEqual(currentPaymentDate, _validTransaction.PaymentDate);
            Assert.Equal(TransactionStatus.AwaitingPayment, _validTransaction.Status);
            Assert.NotEqual(currentStatus, _validTransaction.Status);
            Assert.False(_validTransaction.ItsPaid);
        }

        [Fact(DisplayName = "Atualizar os valores de uma transação existente deve retornar sucesso.")]
        public void ValidTansaction_UpdateValues_ReturnSuccess()
        {
            // Arrange
            var random = new Random();
            var randomNumber = new Random().Next(1, 100);

            var tenantId = Guid.NewGuid();
            var bankAccount = new BankAccount { Id = Guid.NewGuid(), Description = "Santander" };
            var category = new Category(Guid.NewGuid(), "Restaurante", TransactionType.Income);
            var referenceDate = DateTime.Now.AddDays(randomNumber);
            var dueDate = DateTime.Now.AddDays(randomNumber);
            var value = random.Next(1, 10000);
            var description = "New Description";
            var transaction = new Transaction(tenantId, _bankAccount, _category, DateTime.Now, DateTime.Now, 100, "Transaction test");
            transaction.ClearDomainEvents();

            // Act
            transaction.Update(bankAccount, category, referenceDate, dueDate, value, description);

            // Assert
            Assert.Equal(2, transaction.Events.Count());
            Assert.Equal(tenantId, transaction.TenantId);
            Assert.Equal(category.Id, transaction.CategoryId);
            Assert.Equal(bankAccount.Id, transaction.BankAccountId);
            Assert.Equal(referenceDate, transaction.ReferenceDate);
            Assert.Equal(dueDate, transaction.DueDate);
            Assert.Equal(value, transaction.Value);
            Assert.Equal(description, transaction.Description);
        }

        [Fact(DisplayName = "Vincular fatura de cartão de crédito a uma transação deve retornar sucesso.")]
        public void TransactionWithoutCreditCardBill_LinkCreditCardBillIdValid_ReturnSucess()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var creditCardBillId = Guid.NewGuid();
            var category = new Category(tenantId, "Expense", TransactionType.Expense);
            var transaction = new Transaction(tenantId, _bankAccount, category, DateTime.Now, DateTime.Now, 100, "Test");

            // Act
            transaction.LinkCreditCardBill(creditCardBillId);

            // Assert
            Assert.Equal(creditCardBillId, transaction.CreditCardBillId);
        }

        [Fact(DisplayName = "Desvincular fatura de cartão de crédito a uma transação deve retornar sucesso.")]
        public void TransactionWithCreditCardBill_UnlinkCreditCardBill_ReturnSuccess()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var category = new Category(tenantId, "Expense", TransactionType.Expense);
            var transaction = new Transaction(tenantId, _bankAccount, category, DateTime.Now, DateTime.Now, 100, "Test");

            transaction.LinkCreditCardBill(Guid.NewGuid());

            // Act
            transaction.UnlinkCreditCardBill();

            // Assert
            Assert.Null(transaction.CreditCardBillId);
        }

        [Fact(DisplayName = "Vincular fatura de cartão de crédito inválida deve retornar Domain Exception")]
        public void TransactionWithoutCreditCardBill_LinkInvalidCreditCardBillId_ReturnDomainException()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var category = new Category(tenantId, "Expense", TransactionType.Expense);
            var transaction = new Transaction(tenantId, _bankAccount, category, DateTime.Now, DateTime.Now, 100, "Test");

            // Act && Assert
            Assert.Throws<DomainException>(() => transaction.LinkCreditCardBill(Guid.Empty));
        }

        [Fact(DisplayName = "Vincular fatura de cartão de crédito a uma transação que já possui, deve retornar Domain Exception")]
        public void TransactionWithCreditCardBill_LinkCreditCardBillId_ReturnDomainException()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var category = new Category(tenantId, "Expense", TransactionType.Expense);
            var transaction = new Transaction(tenantId, _bankAccount, category, DateTime.Now, DateTime.Now, 100, "Test");
            transaction.LinkCreditCardBill(Guid.NewGuid());

            // Act && Assert
            Assert.Throws<DomainException>(() => transaction.LinkCreditCardBill(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Vincular fatura de cartão de crédito a uma transação já paga, deve retornar Domain Exception")]
        public void TransactionPaid_LinkCreditCardBillId_ReturnDomainException()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var category = new Category(tenantId, "Expense", TransactionType.Expense);
            var transaction = new Transaction(tenantId, _bankAccount, category, DateTime.Now, DateTime.Now, 100, "Test");
            transaction.Pay(DateTime.Now);

            // Act && Assert
            Assert.Throws<DomainException>(() => transaction.LinkCreditCardBill(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Vincular fatura de cartão de crédito a uma transação do tipo receita, deve retornar Domain Exception")]
        public void TransactionIncome_LinkCreditCardBillId_ReturnDomainException()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var category = new Category(tenantId, "Income", TransactionType.Income);
            var transaction = new Transaction(tenantId, _bankAccount, category, DateTime.Now, DateTime.Now, 100, "Test");

            // Act && Assert
            Assert.Throws<DomainException>(() => transaction.LinkCreditCardBill(Guid.NewGuid()));
        }
    }
}