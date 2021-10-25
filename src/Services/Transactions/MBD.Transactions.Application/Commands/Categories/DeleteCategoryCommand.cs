using System;
using FluentValidation;
using FluentValidation.Results;
using MBD.Application.Core.Response;
using MBD.Transactions.Application.Commands.Common;

namespace MBD.Transactions.Application.Commands.Categories
{
    public class DeleteCategoryCommand : Command<IResult>
    {
        public Guid Id { get; private set; }

        public DeleteCategoryCommand(Guid id)
        {
            Id = id;
        }

        public override ValidationResult Validate()
        {
            return new DeleteCategoryCommandValidation().Validate(this);
        }

        public class DeleteCategoryCommandValidation : AbstractValidator<DeleteCategoryCommand>
        {
            public DeleteCategoryCommandValidation()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();
            }
        }
    }
}