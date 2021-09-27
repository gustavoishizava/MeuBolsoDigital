using System;
using FluentValidation;
using FluentValidation.Results;
using MBD.Application.Core.Requests;
using MBD.Core.Enumerations;
using MBD.CreditCards.Domain.Enumerations;

namespace MBD.CreditCards.Application.Requests
{
    public class UpdateCreditCardRequest : BaseRequest
    {
        public Guid Id { get; set; }
        public Guid BankAccountId { get; set; }
        public string Name { get; set; }
        public int ClosingDay { get; set; }
        public int DayOfPayment { get; set; }
        public decimal Limit { get; set; }
        public Brand Brand { get; set; }
        public Status Status { get; set; }

        public override ValidationResult Validate()
        {
            return new UpdateCreditCardValidation().Validate(this);
        }

        public class UpdateCreditCardValidation : AbstractValidator<UpdateCreditCardRequest>
        {
            public UpdateCreditCardValidation()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();

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

                RuleFor(x => x.Status)
                    .IsInEnum();
            }
        }
    }
}