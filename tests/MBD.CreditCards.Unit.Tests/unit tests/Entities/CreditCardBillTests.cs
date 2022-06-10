using System;
using MBD.CreditCards.Domain.Entities;
using MBD.CreditCards.Domain.Enumerations;
using Xunit;

namespace MBD.CreditCards.Unit.Tests.unit_tests.Entities
{
    public class CreditCardBillTests
    {
        private readonly CreditCard _validCreditCard;
        private readonly BankAccount _validBankAccount;
        public CreditCardBillTests()
        {
            var tenantId = Guid.NewGuid();
            _validBankAccount = new BankAccount(Guid.NewGuid(), tenantId, "NuConta");
            _validCreditCard = new CreditCard(tenantId, _validBankAccount, "NuBank", 5, 10, 1000, Brand.VISA);
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
            creditCard.AddTransaction(Guid.NewGuid(), new DateTime(year, month, 1), 100);
            var creditCardBill = creditCard.GetBillByReference(month, year);

            // Assert
            Assert.Equal(creditCard.Id, creditCardBill.CreditCardId);
            Assert.Equal(dueDate, creditCardBill.DueDate);
            Assert.Equal(closesIn, creditCardBill.ClosesIn);
            Assert.Equal(month, creditCardBill.Reference.Month);
            Assert.Equal(year, creditCardBill.Reference.Year);
        }
    }
}