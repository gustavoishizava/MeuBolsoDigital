using System;

namespace MBD.Transactions.Application.Commands
{
    public class CreateTransactionCommand
    {
        public Guid BankAccountId { get; private set; }
        public Guid CategoryId { get; private set; }
        public DateTime ReferenceDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public decimal Value { get; private set; }
        public string Description { get; private set; }

        public CreateTransactionCommand(Guid bankAccountId, Guid categoryId, DateTime referenceDate, DateTime dueDate, DateTime? paymentDate, decimal value, string description)
        {
            BankAccountId = bankAccountId;
            CategoryId = categoryId;
            ReferenceDate = referenceDate;
            DueDate = dueDate;
            PaymentDate = paymentDate;
            Value = value;
            Description = description;
        }
    }
}