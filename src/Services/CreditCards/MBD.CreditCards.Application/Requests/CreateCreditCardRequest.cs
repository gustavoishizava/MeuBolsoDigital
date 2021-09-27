using System;
using FluentValidation;
using FluentValidation.Results;
using MBD.Application.Core.Requests;
using MBD.CreditCards.Domain.Enumerations;

namespace MBD.CreditCards.Application.Requests
{
    public class CreateCreditCardRequest : BaseRequest
    {
        public Guid BankAccountId { get; set; }
        public string Name { get; set; }
        public int ClosingDay { get; set; }
        public int DayOfPayment { get; set; }
        public decimal Limit { get; set; }
        public Brand Brand { get; set; }

        public override ValidationResult Validate()
        {
            return new CreateCreditCardValidation().Validate(this);
        }

        public class CreateCreditCardValidation : AbstractValidator<CreateCreditCardRequest>
        {
            public CreateCreditCardValidation()
            {
                RuleFor(x => x.BankAccountId)
                    .NotEmpty();

                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(100);

                RuleFor(x => x.ClosingDay)
                    .NotEmpty()
                    .GreaterThanOrEqualTo(1)
                    .LessThanOrEqualTo(31);

                RuleFor(x => x.DayOfPayment)
                    .NotEmpty()
                    .GreaterThanOrEqualTo(1)
                    .LessThanOrEqualTo(31);

                RuleFor(x => x.Limit)
                    .NotEmpty()
                    .GreaterThan(0);

                RuleFor(x => x.Brand)
                    .IsInEnum();
            }
        }
    }
}