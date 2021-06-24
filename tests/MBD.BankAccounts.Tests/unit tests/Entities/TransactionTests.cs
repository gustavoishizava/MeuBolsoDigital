using System;
using MBD.BankAccounts.Domain.Entities;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.Core.DomainObjects;
using Xunit;

namespace MBD.BankAccounts.Tests.unit_tests.Entities
{
    public class TransactionTests
    {
        private readonly Account _validAccount;

        public TransactionTests()
        {
            _validAccount = new Account(Guid.NewGuid(), "Conta válida", 1000, AccountType.CheckingAccount);
        }

        [Theory(DisplayName = "Adicionar uma transação válida deve alterar o saldo da conta e retornar valor correto.")]
        [InlineData(500, TransactionType.Income)]
        [InlineData(1000, TransactionType.Income)]
        [InlineData(600, TransactionType.Expense)]
        [InlineData(200, TransactionType.Expense)]
        public void ValidTransaction_AddTransaction_ReturnCorrectBalance(decimal value, TransactionType type)
        {
            // Arrange
            Transaction transaction = null;
            var transactionId = Guid.NewGuid();
            var date = DateTime.Now.AddDays(new Random().Next(1, 100));
            var balance = _validAccount.InitialBalance + (type == TransactionType.Income ? value : value * -1);

            // Act
            _validAccount.AddTransaction(transactionId, date, value, type);
            transaction = _validAccount.GetTransaction(transactionId);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(transactionId, transaction.Id);
            Assert.Equal(_validAccount.Id, transaction.AccountId);
            Assert.Equal(date, transaction.CreatedAt);
            Assert.Equal(value, transaction.Value);
            Assert.Equal(type, transaction.Type);
            Assert.Equal(balance, _validAccount.Balance);
        }

        [Theory(DisplayName = "Adicionar transação com valor inválido deve retornar Domain Exception.")]
        [InlineData(-100)]
        [InlineData(-250)]
        [InlineData(-300)]
        public void InvalidValue_AddTransaction_ReturnDomainException(decimal value)
        {
            // Arrange && Act && Assert
            Assert.Throws<DomainException>(() =>
                _validAccount.AddTransaction(Guid.NewGuid(), DateTime.Now, value, TransactionType.Income));
        }

        [Fact(DisplayName = "Adicionar transações repetidas deve retornar Domain Exception.")]
        public void RepeatedTransaction_AddTransaction_ReturnDomainException()
        {
            // Assert
            var transactionId = Guid.NewGuid();
            _validAccount.AddTransaction(transactionId, DateTime.Now, 100, TransactionType.Income);
            var transaction = _validAccount.GetTransaction(transactionId);

            // Act && Assert
            Assert.Throws<DomainException>(() => 
                _validAccount.AddTransaction(transaction.Id, transaction.CreatedAt, transaction.Value, transaction.Type));
        }
    }
}