using System;
using MBD.Core.DomainObjects;
using MBD.CreditCards.Domain.Entities;
using MBD.CreditCards.Domain.Enumerations;
using Xunit;

namespace MBD.CreditCards.Tests.unit_tests.Entities
{
    public class CreditCardBillTests
    {
        private readonly CreditCard _validCreditCard;
        private readonly BankAccount _validBankAccount;
        public CreditCardBillTests()
        {
            _validBankAccount = new BankAccount();
            _validBankAccount.GetType().GetProperty(nameof(BankAccount.Id)).SetValue(_validBankAccount, Guid.NewGuid());
            _validCreditCard = new CreditCard(Guid.NewGuid(), _validBankAccount, "NuBank", 5, 10, 1000, Brand.VISA);
        }

        [Theory(DisplayName = "Gerar nova fatura com referência válida.")]
        [InlineData(31, 31, 4, 2021)]
        [InlineData(5, 15, 6, 2021)]
        [InlineData(15, 5, 4, 2021)]
        [InlineData(30, 30, 2, 2021)]
        [InlineData(1, 1, 1, 2021)]
        public void ValidReference_NewCreditCardBill_ReturnSuccess(int closingDay, int dayOfPayment, int month, int year)
        {
            // Arrange
            var creditCard = new CreditCard(Guid.NewGuid(), _validBankAccount, "NuBank", closingDay, dayOfPayment, 1000, Brand.VISA);

            var daysInMonth = DateTime.DaysInMonth(year, month);
            DateTime closesIn = DateTime.Now;
            if ((closingDay == 31 || (closingDay > 28 && month == 2)) && daysInMonth < closingDay)
            {
                closesIn = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            }
            else
            {
                closesIn = new DateTime(year, month, creditCard.ClosingDay);
            }

            DateTime dueDate = DateTime.Now;
            if ((dayOfPayment == 31 || (closingDay > 28 && month == 2)) && daysInMonth < dayOfPayment)
            {
                dueDate = new DateTime(year, month, 1).AddMonths(1);
            }
            else
            {
                dueDate = new DateTime(year, month, creditCard.DayOfPayment);
            }

            if (dueDate < closesIn)
                dueDate = dueDate.AddMonths(1);

            // Act
            creditCard.AddBill(month, year);
            var creditCardBill = creditCard.GetBillByReference(month, year);

            // Assert
            Assert.Equal(creditCard.Id, creditCardBill.CreditCardId);
            Assert.Equal(dueDate, creditCardBill.DueDate);
            Assert.Equal(closesIn, creditCardBill.ClosesIn);
            Assert.Equal(month, creditCardBill.Reference.Month);
            Assert.Equal(year, creditCardBill.Reference.Year);
        }

        [Fact(DisplayName = "Adicionar nova fatura com referência válida deve retornar sucesso.")]
        public void ValidReference_AddBill_ReturnSuccess()
        {
            // Arrage
            var random = new Random();
            var month = random.Next(1, 13);
            var year = DateTime.Now.Year;
            CreditCardBill creditCardBill = null;

            // Act
            _validCreditCard.AddBill(month, year);
            creditCardBill = _validCreditCard.GetBillByReference(month, year);

            // Assert
            Assert.NotNull(creditCardBill);
            Assert.Single(_validCreditCard.Bills);
            Assert.Equal(month, creditCardBill.Reference.Month);
            Assert.Equal(year, creditCardBill.Reference.Year);
            Assert.False(_validCreditCard.ReferenceIsAvailable(month, year));
        }

        [Fact(DisplayName = "Adicionar nova fatura com referência repetida válida deve retornar Domain Exception.")]
        public void RepeatedReference_AddBill_ReturnDomainException()
        {
            // Arrage
            var random = new Random();
            var month = random.Next(1, 13);
            var year = DateTime.Now.Year;
            _validCreditCard.AddBill(month, year);

            // Act && Assert
            Assert.Throws<DomainException>(() => _validCreditCard.AddBill(month, year));
        }
    }
}