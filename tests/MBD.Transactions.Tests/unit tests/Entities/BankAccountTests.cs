using System;
using MBD.Transactions.Domain.Entities;
using Xunit;

namespace MBD.Transactions.Tests.unit_tests.Entities
{
    public class BankAccountTests
    {
        [Theory(DisplayName = "Alterar a descrição de uma conta bancária deve retornar sucesso.")]
        [InlineData("Banco 1")]
        [InlineData("Banco 2")]
        public void Create_BankAccount_ReturnSuccess(string description)
        {
            // Arrange
            var id = Guid.NewGuid();
            var tenantId = Guid.NewGuid();

            // Act
            var bankAccount = new BankAccount(id, tenantId, description);

            // Assert
            Assert.Equal(id, bankAccount.Id);
            Assert.Equal(tenantId, bankAccount.TenantId);
            Assert.Equal(description, bankAccount.Description);
        }

        [Theory(DisplayName = "Alterar a descrição de uma conta bancária deve retornar sucesso.")]
        [InlineData("Banco 1")]
        [InlineData("Banco 2")]
        public void SetDescription_ReturnSuccess(string newDescription)
        {
            // Arrange
            var bankAccount = new BankAccount(Guid.NewGuid(), Guid.NewGuid(), "Teste");

            // Act
            bankAccount.SetDescription(newDescription);

            // Assert
            Assert.Equal(newDescription, bankAccount.Description);
        }
    }
}