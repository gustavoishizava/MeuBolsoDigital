using System;
using FluentValidation;
using FluentValidation.Results;
using MBD.Application.Core.Requests;

namespace MBD.Identity.Application.Requests
{
    public class RefreshTokenRequest : BaseRequest
    {
        public Guid RefreshToken { get; set; }

        public override ValidationResult Validate()
        {
            return new RefreshTokenValidation().Validate(this);
        }

        protected class RefreshTokenValidation : AbstractValidator<RefreshTokenRequest>
        {
            public RefreshTokenValidation()
            {
                RuleFor(x => x.RefreshToken)
                    .NotEmpty();
            }
        }
    }
}