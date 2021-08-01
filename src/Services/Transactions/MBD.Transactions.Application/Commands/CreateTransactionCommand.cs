using System;
using FluentValidation;
using FluentValidation.Results;
using MBD.Application.Core.Response;
using MBD.Transactions.Application.Commands.Common;
using MBD.Transactions.Application.Response;

namespace MBD.Transactions.Application.Commands
{
    public class CreateTransactionCommand : Command<IResult<TransactionResponse>>
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

        public override ValidationResult Validate()
        {
            return new CreateTransactionValidation().Validate(this);
        }

        private class CreateTransactionValidation : AbstractValidator<CreateTransactionCommand>
        {
            public CreateTransactionValidation()
            {
                RuleFor(x => x.BankAccountId)
                    .NotEmpty();

                RuleFor(x => x.CategoryId)
                    .NotEmpty();

                RuleFor(x => x.ReferenceDate)
                    .NotEmpty();

                RuleFor(x => x.DueDate)
                    .NotEmpty();

                RuleFor(x => x.Value)
                    .NotEmpty()
                    .GreaterThanOrEqualTo(0);

                RuleFor(x => x.Description)
                    .MaximumLength(100);
            }
        }
    }
}