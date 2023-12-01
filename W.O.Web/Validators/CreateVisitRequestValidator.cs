using FluentValidation;
using W.O.Web.Models;
using W.O.Web.Models.Requests.Create;

namespace W.O.Web.Validators
{
	public sealed class CreateVisitRequestValidator : AbstractValidator<VisitFormDTO>
	{
		public CreateVisitRequestValidator()
		{
			RuleFor(c => c.AssigneeFullName)
				.NotNull()
				.NotEmpty()
				.WithMessage(" \'Assignee name\' is required");

			RuleFor(c => c.AssignedFrom)
				.NotEqual(default(DateTime))
				.WithMessage(" \'Assigned date\' is not valid");


			RuleFor(x => x.Part.Description)
						.NotNull()
						.NotEmpty()
						.WithMessage(" \'Assignee name\' is required");

			RuleFor(x => x.Part.Amount)
				.GreaterThan(0)
				.PrecisionScale(18, 2, false)
				.WithMessage("only 2 scale digits are allowed");

			RuleFor(x => x.Part.Currency)
				 .NotNull().NotEmpty();

			RuleFor(x => x.Part.Quantity)
				 .GreaterThan(0)
				 .WithMessage("Quantity should be greater than 0");

		}

		public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
		{
			var result = await ValidateAsync(ValidationContext<VisitFormDTO>.CreateWithOptions((VisitFormDTO)model, x => x.IncludeProperties(propertyName)));
			if (result.IsValid)
				return Array.Empty<string>();
			return result.Errors.Select(e => e.ErrorMessage);
		};
	}
}
