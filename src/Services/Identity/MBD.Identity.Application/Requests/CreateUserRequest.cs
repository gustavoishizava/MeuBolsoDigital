using FluentValidation;
using FluentValidation.Results;
using MBD.Application.Core.Requests;
using MBD.Core.Constants;

namespace MBD.Identity.Application.Requests
{
    public class CreateUserRequest : BaseRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }

        public override ValidationResult Validate()
        {
            return new CreateUserValidation().Validate(this);
        }

        public class CreateUserValidation : AbstractValidator<CreateUserRequest>
        {
            public CreateUserValidation()
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(100);

                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress();

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .Matches(RegularExpressions.StrongPassword)
                    .Equal(x => x.RepeatPassword);

                RuleFor(x => x.RepeatPassword)
                    .NotEmpty();
            }
        }
    }
}