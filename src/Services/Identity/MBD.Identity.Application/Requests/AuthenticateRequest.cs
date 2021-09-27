using FluentValidation;
using FluentValidation.Results;
using MBD.Application.Core.Requests;

namespace MBD.Identity.Application.Requests
{
    public class AuthenticateRequest : BaseRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public override ValidationResult Validate()
        {
            return new AuthenticateValidation().Validate(this);
        }

        public class AuthenticateValidation : AbstractValidator<AuthenticateRequest>
        {
            public AuthenticateValidation()
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();

                RuleFor(x => x.Password)
                    .NotEmpty();
            }
        }
    }
}