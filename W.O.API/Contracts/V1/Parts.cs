using Azure.Core;
using W.O.API.Domain;
using W.O.API.Domain.Common.Helpers;
using static W.O.API.Contracts.V1.ApiRoutes;

namespace W.O.API.Contracts.V1.Parts;

#region Requests
public record UpdatePartRequest(string? description, decimal? amount, string? currency, int? quantity);

public record CreatePartRequest(Guid visitId, string description, decimal amount, string currency, int quantity)
{
    public static explicit operator Part(CreatePartRequest request)
    {
        return new Part(
            request.visitId,
            request.description, 
            new Price { 
                Amount = request.amount, 
                Currency = CurrencyHelper.GetFromString(request.currency),
            }, 
            request.quantity);
    }
}

public record CreatePartWithinVisitRequest(string description, decimal amount, string currency, int quantity)
{
    public static explicit operator Part(CreatePartWithinVisitRequest dto)
    {
        return new Part(
            Guid.Empty, // will be assigned by Ef core
            dto.description,
            new Price
            {
                Amount = dto.amount,
                Currency = CurrencyHelper.GetFromString(dto.currency)
            },
            dto.quantity);
    }
}
#endregion

#region Responses
public record GetPartResponse(Guid Id, Guid visitId, string description, decimal amount, string currency, int quantity, decimal totalPrice)
{
    public static explicit operator GetPartResponse(Part part)
    {
        return new GetPartResponse(
            part.Id,
            part.VisitId,
            part.Description,
            part.Price.Amount,
            part.Price.Currency.ToString(),
            part.Quantity,
            part.TotalPrice.Amount);
    }
}

public record CreatePartWithinVisitResponse(Guid Id, string description, decimal amount, string currency, int quantity, decimal totalPrice)
{
    public static explicit operator CreatePartWithinVisitResponse(Part part)
    {
        return new CreatePartWithinVisitResponse(
            part.Id,
            part.Description,
            part.Price.Amount,
            part.Price.Currency.ToString(),
            part.Quantity,
            part.TotalPrice.Amount);
    }
}

public record CreatePartResponse(Guid Id, Guid visitId, string description, decimal amount, string currency, int quantity, decimal totalPrice)
{
    public static explicit operator CreatePartResponse(Part part)
    {
        return new CreatePartResponse(
            part.Id,
            part.VisitId,
            part.Description,
            part.Price.Amount,
            part.Price.Currency.ToString(),
            part.Quantity,
            part.TotalPrice.Amount);
    }
}
#endregion


