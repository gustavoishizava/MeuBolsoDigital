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
        public Guid BankAccountId { get; init; }
        public Guid CategoryId { get; init; }
        public DateTime ReferenceDate { get; init; }
        public DateTime DueDate { get; init; }
        public DateTime? PaymentDate { get; init; }
        public decimal Value { get; init; }
        public string Description { get; init; }

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