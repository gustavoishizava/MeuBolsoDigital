using System;
using System.Collections.Generic;
using System.Linq;
using MBD.Core;
using MBD.Core.Entities;
using MBD.Core.Enumerations;
using MBD.CreditCards.Domain.Enumerations;

namespace MBD.CreditCards.Domain.Entities
{
    public class CreditCard : BaseEntity, IAggregateRoot
    {
        private readonly List<CreditCardBill> _bills = new List<CreditCardBill>();

        public Guid UserId { get; private set; }
        public Guid BankAccountId { get; private set; }
        public string Name { get; private set; }
        public int ClosingDay { get; private set; }
        public int DayOfPayment { get; private set; }
        public decimal Limit { get; private set; }
        public Brand Brand { get; private set; }
        public Status Status { get; private set; }

        public IReadOnlyList<CreditCardBill> Bills => _bills.AsReadOnly();

        public CreditCard(Guid userId, Guid bankAccountId, string name, int closingDay, int dayOfPayment, decimal limit, Brand brand)
        {
            Assertions.IsNotNullOrEmpty(name, "Informe um nome.");
            Assertions.HasMaxLength(name, 100, "O nome não deve conter mais que 100 caractes.");
            Assertions.IsBetween(closingDay, 1, 31, "A data de fechamento da fatura deve estar entre os dias 1 - 31.");
            Assertions.IsBetween(dayOfPayment, 1, 31, "A data de pagamento da fatura deve estar entre os dias 1 - 31.");
            Assertions.IsGreaterThan(limit, 0, "O limite deve ser maior que R$0,00.");

            UserId = userId;
            BankAccountId = bankAccountId;
            Name = name;
            ClosingDay = closingDay;
            DayOfPayment = dayOfPayment;
            Limit = limit;
            Brand = brand;

            Status = Status.Active;
        }

        #region Credit card bills

        public void AddBill(int month, int year)
        {
            Assertions.IsTrue(ReferenceIsAvailable(month, year), $"Não é possível criar uma fatura com as referências mês ({month}) e ano ({year}), pois já existe uma fatura cadastrada para esta referência.");
            _bills.Add(new CreditCardBill(Id, DayOfPayment, ClosingDay, month, year));
        }

        public bool ReferenceIsAvailable(int month, int year)
        {
            return !_bills.Any(x => x.Reference.Month == month && x.Reference.Year == year);
        }

        public CreditCardBill GetBillByReference(int month, int year)
        {
            return _bills.FirstOrDefault(x => x.Reference.Month == month && x.Reference.Year == year);
        }

        #endregion        
    }
}