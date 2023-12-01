using FluentValidation;
using System.Data;
using System.Text.RegularExpressions;
using W.O.API.Contracts.V1.Parts;
using W.O.API.Contracts.V1.Visits;
using W.O.API.Contracts.V1.WorkOrders;
using W.O.API.Domain;
using W.O.API.Domain.Common.Helpers;

namespace W.O.API.Validators
{
    public class RequestValidators
    {
        private static bool IsValidGuid(Guid unValidatedGuid)
        {
            if (Guid.TryParse(unValidatedGuid.ToString(), out Guid validatedGuid))
            {
                return validatedGuid != Guid.Empty;
            }
            else
            {
                return false;
            }
        }

        private static bool IsValidDate(DateTime? date)
        {
            if (date is not null)
            {
                return !date.Equals(default(DateTime));
            }
            return true;
        }

        private static bool IsValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }

        public sealed class CreateNoteRequestValidator : AbstractValidator<CreateWorkOrderRequest>
        {
            public CreateNoteRequestValidator()
            {
                RuleFor(c => c.title)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage(" \'Title\' is required");

                RuleFor(c => c.description)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage(" \'Description\' is required");

                RuleFor(p => p.phone)
               .NotEmpty()
               .NotNull().WithMessage("Phone number is required.")
               .MinimumLength(10).WithMessage("Phone number must not be less than 10 characters.")
               .MaximumLength(20).WithMessage("Phone number must not exceed 50 characters.")
               .Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("Phone number is not valid");

                RuleFor(c => c.email)
                    .EmailAddress()
                    .WithMessage(" \'Email\' is not valid");

                RuleFor(c => c.startAt)
                    .Cascade(CascadeMode.Stop)
                    .Must(IsValidDate)
                    .WithMessage(" \'Start date\' is not valid");

                RuleFor(c => c.finishAt)
                    .Cascade(CascadeMode.Stop)
                    .Must(IsValidDate)
                    .WithMessage(" \'Finish date\' is not valid");

                RuleFor(x => x).
                    Must(x=> x.finishAt > x.startAt)
                   .WithMessage("Finish time must be greater than start time");
            }
        }

        public sealed class UpdateWorkOrderRequestValidator : AbstractValidator<UpdateWorkOrderRequest>
        {
            public UpdateWorkOrderRequestValidator()
            {
                RuleFor(c => c.title)
                    .NotEmpty().When(d => d.title != null)
                    .WithMessage("\'Title\' section can not be empty or whitespace");

                RuleFor(c => c.description)
                    .NotEmpty().When(d => d.title != null)
                    .WithMessage("\'Description\' section can not be empty or whitespace");

                RuleFor(c => c.phone)
               .MinimumLength(10).When(d => d.phone != null).WithMessage("Phone number must not be less than 10 characters.")
               .MaximumLength(20).When(d => d.phone != null).WithMessage("Phone number must not exceed 50 characters.")
               .Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).When(d => d.phone != null).WithMessage("Phone number is not valid");

                RuleFor(c => c.email)
                   .EmailAddress().When(d => d.email != null)
                   .WithMessage(" \'Email\' is not valid");

                RuleFor(c => c.startAt)
                    .Cascade(CascadeMode.Stop)
                    .Must(IsValidDate).When(d => d.startAt != null)
                    .WithMessage(" \'Start date\' is not valid");

                RuleFor(c => c.finishAt)
                    .Cascade(CascadeMode.Stop)
                    .Must(IsValidDate).When(d => d.finishAt != null)
                    .WithMessage(" \'Finish date\' is not valid");

                RuleFor(x => x).
                    Must(x => x.finishAt > x.startAt).When(d => d.startAt != null & d.finishAt != null)
                   .WithMessage("Finish time must be greater than start time");
            }
        }

        public sealed class CreateVisitRequestValidator : AbstractValidator<CreateVisitRequest>
        {
            public CreateVisitRequestValidator()
            {
                RuleFor(c => c.workOrderId)
                    .Must(IsValidGuid)
                    .WithMessage("\'Work order id\' must be a valid Guid type ");

                RuleFor(c => c.assigneeFullName)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage(" \'Assignee name\' is required");

                RuleFor(c => c.assignedFrom)
                    .Must(IsValidDate)
                    .WithMessage(" \'assigned date\' is not valid");

                RuleForEach(c => c.parts)
                    .Cascade(CascadeMode.Stop)
                    .NotNull();

                RuleForEach(c => c.parts)
                    .ChildRules(p =>
                {
                    p.RuleFor(x => x.description)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage(" \'Assignee name\' is required");

                    p.RuleFor(x => x.amount)
                    .PrecisionScale(18, 2, false);

                    p.RuleFor(x => x.currency).
                    IsEnumName(typeof(Currency),
                    caseSensitive: false)
                    .WithMessage($"Only {string.Join(", ", CurrencyHelper.GetCurrencyNames())} are allowed as a currency");

                    p.RuleFor(x => x.quantity)
                     .GreaterThan(0)
                     .WithMessage("Quantity should be greater than 0");
                });
            }
        }

        public sealed class UpdateVisitRequestValidator : AbstractValidator<UpdateVisitRequest>
        {
            public UpdateVisitRequestValidator()
            {
                RuleFor(c => c.assigneeFullName)
                    .NotEmpty().When(d => d.assigneeFullName != null)
                    .WithMessage("\'Assignee full name\' section can not be empty or whitespace");

                RuleFor(c => c.assignedFrom)
                    .Cascade(CascadeMode.Stop)
                    .Must(IsValidDate).When(d => d.assignedFrom != null)
                    .WithMessage(" \'Assigned from\' is not valid");
            }
        }

        public sealed class CreatePartRequestValidator : AbstractValidator<CreatePartRequest>
        {
            public CreatePartRequestValidator()
            {
                RuleFor(c => c.visitId)
                    .Must(IsValidGuid)
                    .WithMessage("\'Work order id\' must be a valid Guid type ");

                RuleFor(x => x.description)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage(" \'Assignee name\' is required");

                RuleFor(x => x.amount)
                    .PrecisionScale(2, 18, false);

                RuleFor(x => x.currency).
                    IsEnumName(typeof(Currency),
                    caseSensitive: false)
                    .WithMessage($"Only {string.Join(", ", CurrencyHelper.GetCurrencyNames())} are allowed as a currency");

                RuleFor(x => x.quantity)
                     .Must(x => x.GetType() == typeof(int))
                     .GreaterThan(0)
                     .WithMessage("Quantity must be a positive integer");
            }
        }
    }
}
