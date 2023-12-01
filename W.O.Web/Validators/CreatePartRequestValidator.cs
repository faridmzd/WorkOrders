using FluentValidation;
using System.Text.RegularExpressions;
using W.O.Web.Models.Requests.Create;

namespace W.O.Web.Validators
{
    public class CreatePartRequestValidator :  AbstractValidator<CreatePartRequest>
    {
        public CreatePartRequestValidator()
        {
            RuleFor(x => x.Description)
                        .NotNull()
                        .NotEmpty()
                        .WithMessage(" \'Assignee name\' is required");

            RuleFor(x => x.Amount)
                .PrecisionScale(18, 2, false);

            RuleFor(x => x.Currency)
                .NotNull().NotEmpty();

            RuleFor(x => x.Quantity)
                 .GreaterThan(0)
                 .WithMessage("Quantity should be greater than 0");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<CreatePartRequest>.CreateWithOptions((CreatePartRequest)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
