using System;
using FluentValidation;
using FluentValidation.Results;
using MBD.Application.Core.Requests;
using MBD.Core.Enumerations;

namespace MBD.Transactions.Application.Request
{
    public class UpdateCategoryRequest : BaseRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }

        public override ValidationResult Validate()
        {
            return new UpdateCategoryValidation().Validate(this);
        }

        private class UpdateCategoryValidation : AbstractValidator<UpdateCategoryRequest>
        {
            public UpdateCategoryValidation()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();

                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(100);

                RuleFor(x => x.Status)
                    .IsInEnum();
            }
        }
    }
}