using W.O.API.Contracts.V1.Visits;
using W.O.API.Domain;

namespace W.O.API.Contracts.V1.WorkOrders;

#region Requests
public record UpdateWorkOrderRequest(string? title, string? description, string? phone, string? email, DateTime? startAt, DateTime? finishAt);

public record CreateWorkOrderRequest(string title, string description, string phone, string email, DateTime startAt, DateTime finishAt)
{
    public static explicit operator WorkOrder(CreateWorkOrderRequest request)
    {
        return new WorkOrder(request.title, request.description, request.phone, request.email, request.startAt, request.finishAt);
    }
}
#endregion

#region Responses
public record GetWorkOrderResponse(Guid Id, string title, string description, string phone, string email, 
    DateTime startAt, DateTime finishAt, int totalVisits, int totalParts, IEnumerable<GetVisitResponse> visits)
{
    public static explicit operator GetWorkOrderResponse(WorkOrder order)
    {
        return new GetWorkOrderResponse(
            order.Id,
            order.Title,
            order.Description,
            order.Phone,
            order.Email,
            order.StartAt,
            order.FinishAt,
            order.TotalVisits,
            order.TotalParts,
            order.Visits.Select(v => (GetVisitResponse)v));
    }
}
public record UpdateWorkOrderResponse();
public record CreateWorkOrderResponse(Guid Id, string title, string description, string phone, string email,
    DateTime startAt, DateTime finishAt)
{
    public static explicit operator CreateWorkOrderResponse(WorkOrder order)
    {
        return new CreateWorkOrderResponse(
            order.Id,
            order.Title,
            order.Description,
            order.Phone,
            order.Email,
            order.StartAt,
            order.FinishAt);
    }
}
#endregion


