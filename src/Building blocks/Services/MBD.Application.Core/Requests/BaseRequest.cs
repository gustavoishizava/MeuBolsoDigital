using FluentValidation.Results;

namespace MBD.Application.Core.Requests
{
    public abstract class BaseRequest
    {
        public abstract ValidationResult Validate();
    }
}