using System;
using MBD.BankAccounts.Domain.Entities;
using MBD.BankAccounts.Domain.Enumerations;
using MBD.Core.DomainObjects;
using MBD.Core.Enumerations;
using Xunit;

namespace MBD.BankAccounts.Tests.unit_tests.Entities
{
    public class AccountTests
    {
        [Theory(DisplayName = "Criar conta com saldo e descrição inválida deve retornar Domain Exception.")]
        [InlineData("", 0)]
        [InlineData("Banco do Brasil", -1)]
        public void InvalidBalanceAndDescription_NewAccount_ReturnDomainException(string description, decimal initialBalance)
        {
            // Arrange && Act && Assert
            Assert.Throws<DomainException>(() =>
                new Account(Guid.NewGuid(), description, initialBalance, AccountType.CheckingAccount));
        }

        [Fact(DisplayName = "Criar conta com descrição excedendo o limite de tamanho deve retornar Domain Exception.")]
        public void InvalidDescriptionLength_NewAccount_ReturnDomainException()
        {
            // Arrange
            var invalidDescription = new String('a', 151);

            // Act && Assert
            Assert.Throws<DomainException>(() =>
                new Account(Guid.NewGuid(), invalidDescription, 0, AccountType.CheckingAccount));
        }

        [Theory(DisplayName = "Criar conta com descrição e saldo inicial válido deve retornar sucesso.")]
        [InlineData("Banco do Brasil", 0)]
        [InlineData("NuBank", 100)]
        [InlineData("Itaú", 5000)]
        [InlineData("Santander", 0.1)]
        public void ValidDescriptionAndBalance_NewAccount_ReturnSuccess(string description, decimal initialBalance)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountType = AccountType.CheckingAccount;

            // Act
            var account = new Account(userId, description, initialBalance, accountType);

            // Assert
            Assert.NotNull(account);
            Assert.Equal(userId, account.UserId);
            Assert.Equal(description, account.Description);
            Assert.Equal(initialBalance, account.InitialBalance);
            Assert.Equal(AccountType.CheckingAccount, account.Type);
            Assert.Equal(Status.Active, account.Status);
        }
    }
}