using System;
using FluentValidation;
using FluentValidation.Results;
using MBD.Application.Core.Response;
using MBD.Transactions.Application.Commands.Common;

namespace MBD.Transactions.Application.Commands
{
    public class DeleteTransactionCommand : Command<IResult>
    {
        public Guid Id { get; private set; }

        public DeleteTransactionCommand(Guid id)
        {
            Id = id;
        }

        public override ValidationResult Validate()
        {
            return new DeleteTransactionValidation().Validate(this);
        }

        public class DeleteTransactionValidation : AbstractValidator<DeleteTransactionCommand>
        {
            public DeleteTransactionValidation()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();
            }
        }
    }
}