using System;
using MBD.Core.DomainObjects;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Enumerations;
using Xunit;

namespace MBD.Transactions.Tests.unit_tests.Entities
{
    public class TransactionTests
    {
        private readonly Transaction _validTransaction;

        public TransactionTests()
        {
            _validTransaction = new Transaction(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(5), 100, string.Empty);
        }

        [Theory(DisplayName = "Criar transação com valor inválido deve retornar Domain Exception.")]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        public void InvalidValue_NewTransaction_ReturnDomainException(decimal value)
        {
            // Arrange && Act && Assert
            Assert.Throws<DomainException>(() => new Transaction(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now, value, string.Empty));
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
            var userId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var referenceDate = DateTime.Now;
            var dueDate = referenceDate.AddDays(5);

            // Act
            var transaction = new Transaction(userId, bankAccountId, categoryId, referenceDate, dueDate, value, description);

            // Assert
            Assert.Equal(userId, transaction.UserId);
            Assert.Equal(bankAccountId, transaction.BankAccountId);
            Assert.Equal(categoryId, transaction.CategoryId);
            Assert.Equal(referenceDate, transaction.ReferenceDate);
            Assert.Equal(dueDate, transaction.DueDate);
            Assert.Equal(TransactionStatus.AwaitingPayment, transaction.Status);
            Assert.Equal(description, transaction.Description);
            Assert.Equal(value, transaction.Value);
            Assert.Null(transaction.PaymentDate);
        }

        [Fact(DisplayName = "Efetuar o pagamento de uma transação válida deve retornar sucesso.")]
        public void ValidTransaction_ProcessPayment_ReturnSuccess()
        {
            // Arrange
            var paymentDate = _validTransaction.DueDate;

            // Act
            _validTransaction.Pay(paymentDate);

            // Assert
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

            // Act
            _validTransaction.UndoPayment();

            // Assert
            Assert.Null(_validTransaction.PaymentDate);
            Assert.NotEqual(currentPaymentDate, _validTransaction.PaymentDate);
            Assert.Equal(TransactionStatus.AwaitingPayment, _validTransaction.Status);
            Assert.NotEqual(currentStatus, _validTransaction.Status);
            Assert.False(_validTransaction.ItsPaid);
        }
    }
}