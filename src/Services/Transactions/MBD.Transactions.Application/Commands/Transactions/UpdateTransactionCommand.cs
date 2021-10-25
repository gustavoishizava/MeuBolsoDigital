using System;
using FluentValidation;
using FluentValidation.Results;
using MBD.Application.Core.Response;
using MBD.Transactions.Application.Commands.Common;

namespace MBD.Transactions.Application.Commands.Transactions
{
    public class UpdateTransactionCommand : Command<IResult>
    {
        public Guid Id { get; init; }
        public Guid BankAccountId { get; init; }
        public Guid CategoryId { get; init; }
        public DateTime ReferenceDate { get; init; }
        public DateTime DueDate { get; init; }
        public DateTime? PaymentDate { get; init; }
        public decimal Value { get; init; }
        public string Description { get; init; }

        public UpdateTransactionCommand(Guid id, Guid bankAccountId, Guid categoryId, DateTime referenceDate, DateTime dueDate, DateTime? paymentDate, decimal value, string description)
        {
            Id = id;
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
            return new UpdateTransactionValidation().Validate(this);
        }

        public class UpdateTransactionValidation : AbstractValidator<UpdateTransactionCommand>
        {
            public UpdateTransactionValidation()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();

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