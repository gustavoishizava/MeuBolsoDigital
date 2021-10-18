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
            _validTransaction = new Transaction(Guid.NewGuid(), _bankAccount, _category, DateTime.Now, DateTime.Now.AddDays(5), 100, string.Empty, null);
        }

        [Theory(DisplayName = "Criar transação com valor inválido deve retornar Domain Exception.")]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        public void InvalidValue_NewTransaction_ReturnDomainException(decimal value)
        {
            // Arrange && Act && Assert
            Assert.Throws<DomainException>(() => new Transaction(Guid.NewGuid(), _bankAccount, _category, DateTime.Now, DateTime.Now, value, string.Empty, null));
        }

        [Theory(DisplayName = "Criar transação com parâmetros válidos deve retornar sucesso.")]
        [InlineData(0, "Zero", true)]
        [InlineData(10, "Dez", false)]
        [InlineData(100.50, "Cem", true)]
        [InlineData(1000, "Mil", false)]
        [InlineData(10000.50, "Dez mil", true)]
        public void ValidParameters_NewTransaction_ReturnSuccess(decimal value, string description, bool pay)
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var referenceDate = DateTime.Now;
            var dueDate = referenceDate.AddDays(5);
            DateTime? paymentDate = pay ? DateTime.Now : null;
            var eventsCount = pay ? 2 : 1;
            var status = pay ? TransactionStatus.Paid : TransactionStatus.AwaitingPayment;

            // Act
            var transaction = new Transaction(tenantId, _bankAccount, _category, referenceDate, dueDate, value, description, paymentDate);

            // Assert
            Assert.Equal(tenantId, transaction.TenantId);
            Assert.Equal(_bankAccount.Id, transaction.BankAccountId);
            Assert.Equal(_category.Id, transaction.CategoryId);
            Assert.Equal(_category, transaction.Category);
            Assert.Equal(referenceDate, transaction.ReferenceDate);
            Assert.Equal(dueDate, transaction.DueDate);
            Assert.Equal(description, transaction.Description);
            Assert.Equal(value, transaction.Value);
            Assert.Equal(paymentDate, transaction.PaymentDate);
            Assert.Equal(pay, transaction.ItsPaid);
            Assert.Equal(status, transaction.Status);
            Assert.Null(transaction.CreditCardBillId);
            Assert.Equal(eventsCount, transaction.Events.Count);
        }

        [Theory(DisplayName = "Atualizar os valores de uma transação existente deve retornar sucesso.")]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void ValidTansaction_UpdateValues_ReturnSuccess(bool itsPaid, bool pay)
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
            var eventCount = itsPaid == pay ? 2 : 3;
            if (itsPaid is true && pay is true)
                eventCount = 3;

            DateTime? paymentDate = itsPaid ? DateTime.Now : null;
            var currentStatus = itsPaid ? TransactionStatus.Paid : TransactionStatus.AwaitingPayment;

            DateTime? newPaymenteDate = pay ? DateTime.Now : null;
            var expectedStatus = pay ? TransactionStatus.Paid : TransactionStatus.AwaitingPayment;

            var transaction = new Transaction(tenantId, _bankAccount, _category, DateTime.Now, DateTime.Now, 100, "Transaction test", paymentDate);
            transaction.ClearDomainEvents();

            // Act
            transaction.Update(bankAccount, category, referenceDate, dueDate, value, description, newPaymenteDate);

            // Assert
            Assert.Equal(eventCount, transaction.Events.Count());
            Assert.Equal(tenantId, transaction.TenantId);
            Assert.Equal(category.Id, transaction.CategoryId);
            Assert.Equal(bankAccount.Id, transaction.BankAccountId);
            Assert.Equal(referenceDate, transaction.ReferenceDate);
            Assert.Equal(dueDate, transaction.DueDate);
            Assert.Equal(value, transaction.Value);
            Assert.Equal(description, transaction.Description);
            Assert.Equal(expectedStatus, transaction.Status);

            if (itsPaid == pay)
                Assert.Equal(currentStatus, transaction.Status);

            if (itsPaid != pay)
                Assert.NotEqual(currentStatus, transaction.Status);
        }

        [Fact(DisplayName = "Vincular fatura de cartão de crédito a uma transação deve retornar sucesso.")]
        public void TransactionWithoutCreditCardBill_LinkCreditCardBillIdValid_ReturnSucess()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var creditCardBillId = Guid.NewGuid();
            var category = new Category(tenantId, "Expense", TransactionType.Expense);
            var transaction = new Transaction(tenantId, _bankAccount, category, DateTime.Now, DateTime.Now, 100, "Test", null);
            transaction.ClearDomainEvents();

            // Act
            transaction.LinkCreditCardBill(creditCardBillId);

            // Assert
            Assert.Single(transaction.Events);
            Assert.Equal(creditCardBillId, transaction.CreditCardBillId);
        }

        [Fact(DisplayName = "Desvincular fatura de cartão de crédito a uma transação deve retornar sucesso.")]
        public void TransactionWithCreditCardBill_UnlinkCreditCardBill_ReturnSuccess()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var category = new Category(tenantId, "Expense", TransactionType.Expense);
            var transaction = new Transaction(tenantId, _bankAccount, category, DateTime.Now, DateTime.Now, 100, "Test", null);

            transaction.LinkCreditCardBill(Guid.NewGuid());
            transaction.ClearDomainEvents();

            // Act
            transaction.UnlinkCreditCardBill();

            // Assert
            Assert.Single(transaction.Events);
            Assert.Null(transaction.CreditCardBillId);
        }

        [Fact(DisplayName = "Vincular fatura de cartão de crédito inválida deve retornar Domain Exception")]
        public void TransactionWithoutCreditCardBill_LinkInvalidCreditCardBillId_ReturnDomainException()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var category = new Category(tenantId, "Expense", TransactionType.Expense);
            var transaction = new Transaction(tenantId, _bankAccount, category, DateTime.Now, DateTime.Now, 100, "Test", null);

            // Act && Assert
            Assert.Throws<DomainException>(() => transaction.LinkCreditCardBill(Guid.Empty));
        }

        [Fact(DisplayName = "Vincular fatura de cartão de crédito a uma transação que já possui, deve retornar Domain Exception")]
        public void TransactionWithCreditCardBill_LinkCreditCardBillId_ReturnDomainException()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var category = new Category(tenantId, "Expense", TransactionType.Expense);
            var transaction = new Transaction(tenantId, _bankAccount, category, DateTime.Now, DateTime.Now, 100, "Test", null);
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
            var transaction = new Transaction(tenantId, _bankAccount, category, DateTime.Now, DateTime.Now, 100, "Test", DateTime.Now);

            // Act && Assert
            Assert.Throws<DomainException>(() => transaction.LinkCreditCardBill(Guid.NewGuid()));
        }

        [Fact(DisplayName = "Vincular fatura de cartão de crédito a uma transação do tipo receita, deve retornar Domain Exception")]
        public void TransactionIncome_LinkCreditCardBillId_ReturnDomainException()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var category = new Category(tenantId, "Income", TransactionType.Income);
            var transaction = new Transaction(tenantId, _bankAccount, category, DateTime.Now, DateTime.Now, 100, "Test", null);

            // Act && Assert
            Assert.Throws<DomainException>(() => transaction.LinkCreditCardBill(Guid.NewGuid()));
        }
    }
}