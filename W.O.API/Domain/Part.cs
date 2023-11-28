using Azure.Core;
using System.ComponentModel.DataAnnotations.Schema;
using W.O.API.Domain.Common;
using W.O.API.Domain.Common.Exceptions;
using W.O.API.Domain.Common.Helpers;

namespace W.O.API.Domain
{
    [Table("Parts")]
    public class Part : Base
    {
        public Part(Guid visitId, string description, Price price, int quantity)
        {
            ArgumentException.ThrowIfNullOrEmpty(description, nameof(description));

            this.VisitId = visitId;
            this.Description = description;
            this.Price = price;
            this.Quantity = quantity;
        }

        private Part() { }

        public Guid VisitId { get; private set; }
        public string Description { get; private set; }
        public int Quantity { get; private set; }

        [NotMapped]
        public Price Price { get; private set; }

        [NotMapped]
        public Price TotalPrice => new Price { Currency = this.Price.Currency, Amount = this.Price.Amount * Quantity };

        public Part Update(string? description, decimal? amount, string? currency, int? quantity)
        {
            Description = description ?? this.Description;
            Price.Amount = amount ?? this.Price.Amount;
            Price.Currency = currency != null
                ? CurrencyHelper.GetFromString(currency)
                : this.Price.Currency;
            Quantity = quantity ?? this.Quantity;

            return this;
        }
    }
}

