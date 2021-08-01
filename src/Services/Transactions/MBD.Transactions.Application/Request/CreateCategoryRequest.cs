using System;
using FluentValidation;
using FluentValidation.Results;
using MBD.Application.Core.Requests;
using MBD.Transactions.Domain.Enumerations;

namespace MBD.Transactions.Application.Request
{
    public class CreateCategoryRequest : BaseRequest
    {
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public TransactionType Type { get; set; }

        public override ValidationResult Validate()
        {
            return new CreateCategoryValidation().Validate(this);
        }

        private class CreateCategoryValidation : AbstractValidator<CreateCategoryRequest>
        {
            public CreateCategoryValidation()
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(100);

                RuleFor(x => x.Type)
                    .IsInEnum();
            }
        }
    }
}