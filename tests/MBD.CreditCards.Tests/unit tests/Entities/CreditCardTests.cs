using System;
using MBD.Core.DomainObjects;
using MBD.Core.Enumerations;
using MBD.CreditCards.Domain.Entities;
using MBD.CreditCards.Domain.Enumerations;
using Xunit;

namespace MBD.CreditCards.Tests.unit_tests.Entities
{
    public class CreditCardTests
    {
        private readonly BankAccount _validBankAccount;
        public CreditCardTests()
        {
            _validBankAccount = new BankAccount();
            _validBankAccount.GetType().GetProperty(nameof(BankAccount.Id)).SetValue(_validBankAccount, Guid.NewGuid());
        }

        [Theory(DisplayName = "Criar um novo cartão de crédito com dados inválidos.")]
        [InlineData("", 1, 1, 100)]
        [InlineData("Cartão", 0, 1, 100)]
        [InlineData("Cartão", 32, 1, 100)]
        [InlineData("Cartão", 1, 0, 100)]
        [InlineData("Cartão", 1, 32, 100)]
        [InlineData("Cartão", 31, 31, 0)]
        public void InvalidParameters_NewCreditCard_ReturnDomainException(string name, int closingDay, int dayOfPayment, decimal limit)
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var bankAccountId = Guid.NewGuid();

            // Act && Assert
            Assert.Throws<DomainException>(() =>
                new CreditCard(tenantId, _validBankAccount, name, closingDay, dayOfPayment, limit, Brand.VISA));

            if (string.IsNullOrEmpty(name))
            {
                var invalidName = new String('a', 101);

                Assert.Throws<DomainException>(() =>
                    new CreditCard(tenantId, _validBankAccount, invalidName, closingDay, dayOfPayment, limit, Brand.VISA));
            }
        }

        [Theory(DisplayName = "Criar novo cartão de crédito com dados válidos.")]
        [InlineData("NuBank", 10, 15, 5000)]
        [InlineData("NuBank Gold", 31, 2, 15000)]
        [InlineData("NuBank Platinum", 1, 31, 100)]
        public void ValidParameters_NewCreditCard_ReturnSuccess(string name, int closingDay, int dayOfPayment, decimal limit)
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var brand = Brand.VISA;

            // Act
            var creditCard = new CreditCard(tenantId, _validBankAccount, name, closingDay, dayOfPayment, limit, brand);

            //Then
            Assert.Equal(tenantId, creditCard.TenantId);
            Assert.Equal(_validBankAccount.Id, creditCard.BankAccount.Id);
            Assert.Equal(name, creditCard.Name);
            Assert.Equal(closingDay, creditCard.ClosingDay);
            Assert.Equal(dayOfPayment, creditCard.DayOfPayment);
            Assert.Equal(limit, creditCard.Limit);
            Assert.Equal(brand, creditCard.Brand);
            Assert.Equal(Status.Active, creditCard.Status);
            Assert.Empty(creditCard.Bills);
        }

        [Theory(DisplayName = "Alterar o status de um cartão válido deve retornar sucesso.")]
        [InlineData(true)]
        [InlineData(false)]
        public void ValidCreditCard_SetStatus_ReturnSuccess(bool activate)
        {
            // Arrange
            var creditCard = new CreditCard(Guid.NewGuid(), _validBankAccount, "Credit card", 5, 10, 1000, Brand.VISA);
            var expectedStatus = activate ? Status.Active : Status.Inactive;

            // Act
            if (activate)
                creditCard.Activate();
            else
                creditCard.Deactivate();

            // Assert
            Assert.Equal(expectedStatus, creditCard.Status);
        }
    }
}