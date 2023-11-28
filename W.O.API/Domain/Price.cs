using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using W.O.API.Domain.Common;

namespace W.O.API.Domain
{
    [NotMapped]
    public class Price : ValueObject
    {
        public decimal Amount { get; set; }

        public Currency Currency { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return [Amount, Currency];
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Currency
    {
        USD = 0,
        EUR = 1,
        GBP = 2,
        PLN = 4,
        UAH = 8
    }
}
