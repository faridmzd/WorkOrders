using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace W.O.API.Domain.Common.Helpers
{
    public static class CurrencyHelper
    {
        public static string[] GetCurrencyNames() 
        {
            return Enum.GetNames(typeof(Currency));
        }

        public static Currency GetFromString(string value)
        {
            return (Currency)Enum.Parse(typeof(Currency), value, true);
        }

    }

    public static class ValidatorExtensions
    {
        public static void AddToModelState(this ValidationResult validationResult, ModelStateDictionary modelState)
        {

            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

        }
    }
}
