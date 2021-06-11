using System;
using MBD.Core;

namespace MBD.CreditCards.Domain.ValueObjects
{
    public class BillReference
    {
        public int Month { get; private set; }
        public int Year { get; private set; }

        public BillReference(int month, int year)
        {
            Assertions.IsBetween(month, 1, 12, "Informe um mês de referência válido.");
            Assertions.IsBetween(year, 1900, 2999, "Informe um ano de referência válido.");

            Month = month;
            Year = year;
        }

        public DateTime GetClosingDate(int day)
        {
            var date = GetDate(day);
            if (date.Month > Month)
                date = date.AddDays(-1);

            return date;
        }

        public DateTime GetDueDate(int day)
        {
            return GetDate(day);
        }

        private DateTime GetDate(int day)
        {
            Assertions.IsBetween(day, 1, 31, "Informe um dia de fechamento válido.");

            var daysInMonth = DateTime.DaysInMonth(Year, Month);
            if ((day == 31 || (day > 28 && Month == 2)) && daysInMonth < day)
            {
                return new DateTime(Year, Month, 1).AddMonths(1);
            }

            return new DateTime(Year, Month, day);
        }
    }
}