using FluentValidation;
using System.Text.RegularExpressions;
using W.O.Web.Models;
using W.O.Web.Models.Requests.Create;

namespace W.O.Web.Validators
{
    public class CreateWorkOrderRequestValidator : AbstractValidator<CreateWorkOrderRequest>
    {
        public CreateWorkOrderRequestValidator() 
        {
            RuleFor(c => c.Title)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage(" \'Title\' is required");

            RuleFor(c => c.Description)
                .NotNull()
                .NotEmpty()
                .WithMessage(" \'Description\' is required");

            RuleFor(p => p.Phone)
           .NotEmpty()
           .NotNull().WithMessage("Phone number is required.")
           .Length(15).WithMessage("Phone number must be 15 characters.(XX-XXX-XXX-XXXX)")
           .Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("Phone number is not valid");

            RuleFor(c => c.Email)
                   .NotEmpty()
                   .NotNull()
                   .EmailAddress()
                   .WithMessage(" \'Email\' is not valid");

            RuleFor(c => c.StartAt)
                .NotEqual(default(DateTime))
                .WithMessage(" \'Start date\' is not valid");

            RuleFor(c => c.FinishAt)
                .NotEqual(default(DateTime))
                .GreaterThan(c=>c.StartAt)
                .WithMessage(" \'Finish date\' is not valid");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<CreateWorkOrderRequest>.CreateWithOptions((CreateWorkOrderRequest)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
