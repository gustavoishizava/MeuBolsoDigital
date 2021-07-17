using System;
using MBD.Core.DomainObjects;
using MBD.CreditCards.Domain.Entities;
using MBD.CreditCards.Domain.Enumerations;
using Xunit;

namespace MBD.CreditCards.Tests.unit_tests.Entities
{
    public class TransactionTests
    {
        private readonly CreditCardBill _validBill;

        public TransactionTests()
        {
            var creditCard = new CreditCard(Guid.NewGuid(), Guid.NewGuid(), "Cartão", 1, 5, 100, Brand.VISA);
            _validBill = creditCard.CreateCreditCardBill(DateTime.Now.Month, DateTime.Now.Year);
        }

        [Theory(DisplayName = "Adicionar nova transação com sucesso.")]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public void ValidTransaction_AddTransaction_ReturnSucess(decimal value)
        {
            // Arrange
            Transaction transaction = null;
            var transactionId = Guid.NewGuid();
            var correctBalance = value;

            // Act
            _validBill.AddTransaction(transactionId, value);
            transaction = _validBill.GetTransaction(transactionId);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(transactionId, transaction.Id);
            Assert.Equal(_validBill.Id, transaction.CreditCardBillId);
            Assert.True(DateTime.Now >= transaction.CreatedAt);
            Assert.Equal(value, transaction.Value);
            Assert.Equal(correctBalance, _validBill.Balance);
        }

        [Fact(DisplayName = "Adicionar transações repetidas deve retornar Domain Exception.")]
        public void RepeatedTransaction_AddTransaction_ReturnDomainException()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            _validBill.AddTransaction(transactionId, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() => _validBill.AddTransaction(transactionId, 0));
        }

        [Theory(DisplayName = "Adicionar transação com valor inválido deve retornar Domain Exception.")]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-100)]
        [InlineData(-1000)]
        public void InvalidValue_AddTransaction_ReturnDomainException(decimal invalidValue)
        {
            // Arrange && Act && Assert
            Assert.Throws<DomainException>(() => _validBill.AddTransaction(Guid.NewGuid(), invalidValue));
        }
    }
}