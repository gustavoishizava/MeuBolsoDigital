using System;
using MBD.Core.DomainObjects;
using MBD.CreditCards.Domain.Entities;
using MBD.CreditCards.Domain.Enumerations;
using Xunit;

namespace MBD.CreditCards.Unit.Tests.unit_tests.Entities
{
    public class TransactionTests
    {
        private readonly CreditCard _validCreditCard;

        public TransactionTests()
        {
            var tenantId = Guid.NewGuid();
            var validBankAccount = new BankAccount(Guid.NewGuid(), tenantId, "NuConta");
            _validCreditCard = new CreditCard(tenantId, validBankAccount, "Cartão", 1, 5, 100, Brand.VISA);
        }

        [Theory(DisplayName = "Adicionar nova transação em uma fatura não existente com sucesso.")]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public void ValidTransaction_BillNotExists_AddTransaction_ReturnSucess(decimal value)
        {
            // Arrange
            Transaction transaction = null;
            CreditCardBill bill = null;
            var transactionId = Guid.NewGuid();
            var correctBalance = value;
            var createdAt = DateTime.Now;

            // Act
            _validCreditCard.AddTransaction(transactionId, createdAt, value);
            transaction = _validCreditCard.GetTransaction(transactionId, createdAt.Month, createdAt.Year);
            bill = _validCreditCard.GetBillByReference(createdAt.Month, createdAt.Year);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(transactionId, transaction.Id);
            Assert.Equal(bill.Id, transaction.CreditCardBillId);
            Assert.True(DateTime.Now >= transaction.CreatedAt);
            Assert.Equal(value, transaction.Value);
            Assert.Equal(correctBalance, bill.Balance);
            Assert.Single(bill.Transactions);
            Assert.Single(_validCreditCard.Bills);
        }

        [Theory(DisplayName = "Adicionar nova transação em uma fatura já existente com sucesso.")]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public void ValidTransaction_BillExists_AddTransaction_ReturnSucess(decimal value)
        {
            // Arrange
            Transaction transaction = null;
            CreditCardBill bill = null;
            var transactionId = Guid.NewGuid();
            var defaultValue = 10;
            var correctBalance = defaultValue + value;
            var createdAt = DateTime.Now;

            _validCreditCard.AddTransaction(Guid.NewGuid(), createdAt, defaultValue);

            // Act
            _validCreditCard.AddTransaction(transactionId, createdAt, value);
            transaction = _validCreditCard.GetTransaction(transactionId, createdAt.Month, createdAt.Year);
            bill = _validCreditCard.GetBillByReference(createdAt.Month, createdAt.Year);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(transactionId, transaction.Id);
            Assert.Equal(bill.Id, transaction.CreditCardBillId);
            Assert.True(DateTime.Now >= transaction.CreatedAt);
            Assert.Equal(value, transaction.Value);
            Assert.Equal(correctBalance, bill.Balance);
            Assert.Equal(2, bill.Transactions.Count);
            Assert.Single(_validCreditCard.Bills);
        }

        [Fact(DisplayName = "Adicionar transações repetidas não deve fazer nada.")]
        public void RepeatedTransaction_AddTransaction_DoNothing()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var createdAt = DateTime.Now;
            _validCreditCard.AddTransaction(transactionId, createdAt, 100);
            CreditCardBill bill = _validCreditCard.GetBillByReference(createdAt.Month, createdAt.Year);

            // Act
            _validCreditCard.AddTransaction(transactionId, createdAt, 100);

            // Assert
            Assert.Single(bill.Transactions);
        }

        [Fact(DisplayName = "Obter transação de uma fatura não existente deve retornar null.")]
        public void GetTransaction_BillNotExists_ReturnNull()
        {
            // Arrange && Act
            var transaction = _validCreditCard.GetTransaction(Guid.NewGuid(), DateTime.Now.Month, DateTime.Now.Year);

            // Assert
            Assert.Null(transaction);
        }

        [Fact(DisplayName = "Obter transação não existente deve retornar null.")]
        public void GetTransaction_TransactionNotExists_ReturnNull()
        {
            // Arrange
            _validCreditCard.AddTransaction(Guid.NewGuid(), DateTime.Now, 100);

            // Act
            var transaction = _validCreditCard.GetTransaction(Guid.NewGuid(), DateTime.Now.Month, DateTime.Now.Year);

            // Assert
            Assert.Null(transaction);
        }

        [Fact(DisplayName = "Obter transação existente.")]
        public void GetTransaction_TransactionExists_ReturnTransaction()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var createdAt = DateTime.Now;
            var value = 100;
            _validCreditCard.AddTransaction(transactionId, createdAt, value);

            // Act
            var transaction = _validCreditCard.GetTransaction(transactionId, createdAt.Month, createdAt.Year);

            // Assert
            Assert.Equal(transactionId, transaction.Id);
            Assert.Equal(createdAt, transaction.CreatedAt);
            Assert.Equal(value, transaction.Value);
        }

        [Theory(DisplayName = "Adicionar transação com valor inválido deve retornar Domain Exception.")]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        [InlineData(-1000)]
        public void InvalidValue_AddTransaction_ReturnDomainException(decimal invalidValue)
        {
            // Arrange && Act && Assert
            Assert.Throws<DomainException>(() => _validCreditCard.AddTransaction(Guid.NewGuid(), DateTime.Now, invalidValue));
        }
    }
}