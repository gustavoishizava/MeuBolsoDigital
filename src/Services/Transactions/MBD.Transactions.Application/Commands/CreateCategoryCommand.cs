using System;
using FluentValidation;
using FluentValidation.Results;
using MBD.Application.Core.Response;
using MBD.Transactions.Application.Commands.Common;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Enumerations;

namespace MBD.Transactions.Application.Commands
{
    public class CreateCategoryCommand : Command<IResult<CategoryResponse>>
    {
        public Guid? ParentCategoryId { get; init; }
        public string Name { get; init; }
        public TransactionType Type { get; init; }

        public override ValidationResult Validate()
        {
            return new CreateCategoryCommandValidation().Validate(this);
        }

        public class CreateCategoryCommandValidation : AbstractValidator<CreateCategoryCommand>
        {
            public CreateCategoryCommandValidation()
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(100);

                RuleFor(x => x.Type)
                    .IsInEnum();
            }
        }
    }
}