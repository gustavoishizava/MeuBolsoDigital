using System;
using FluentValidation.Results;

namespace MBD.Application.Core.Requests
{
    public abstract class BaseRequest
    {
        public virtual ValidationResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}