using System;
using MBD.Core;
using MBD.Core.Entities;
using MBD.CreditCards.Domain.ValueObjects;

namespace MBD.CreditCards.Domain.Entities
{
    public class CreditCardBill : BaseEntity, IAggregateRoot
    {
        public Guid CreditCardId { get; private set; }
        public DateTime ClosesIn { get; private set; }
        public DateTime DueDate { get; private set; }
        public BillReference Reference { get; private set; }

        internal CreditCardBill(Guid creditCardId, int paymentDay, int closingDay, int month, int year)
        {
            CreditCardId = creditCardId;
            Reference = new BillReference(month, year);
            SetDates(paymentDay, closingDay, month, year);
        }

        private void SetDates(int paymentDay, int closingDay, int month, int year)
        {
            Assertions.IsNotNull(Reference, "Informe a referência da fatura.");

            DueDate = Reference.GetDueDate(paymentDay);
            ClosesIn = Reference.GetClosingDate(closingDay);
            AdjustDueDate();
        }

        private void AdjustDueDate()
        {
            if(DueDate >= ClosesIn)
                return;
            
            DueDate = DueDate.AddMonths(1);
        }
    }
}