using Azure.Core;

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
}
