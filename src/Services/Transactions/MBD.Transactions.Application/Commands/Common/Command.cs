using FluentValidation.Results;
using MBD.Application.Core.Response;
using MediatR;

namespace MBD.Transactions.Application.Commands.Common
{
    public abstract class Command<TCommandResult> : IRequest<TCommandResult> where TCommandResult : IResult
    {
        public abstract ValidationResult Validate();
    }
}