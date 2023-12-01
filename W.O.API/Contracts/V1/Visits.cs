using W.O.API.Contracts.V1.Parts;
using W.O.API.Domain;
using static W.O.API.Contracts.V1.ApiRoutes;

namespace W.O.API.Contracts.V1.Visits;

#region Requests
public record UpdateVisitRequest(string? assigneeFullName, DateTime? assignedFrom);

public record CreateVisitRequest(Guid workOrderId, string assigneeFullName, DateTime assignedFrom, ICollection<CreatePartWithinVisitRequest> parts)
{
    public static explicit operator Visit(CreateVisitRequest request)
    {
        return new Visit(
            request.workOrderId,
            request.assigneeFullName,
            request.assignedFrom,
            request.parts.Select(p => (Part)p ).ToList());
    }
}
#endregion

#region Responses
public record GetVisitResponse(Guid Id, Guid workOrderId, string assigneeFullName, DateTime assignedFrom, IEnumerable<GetPartResponse> parts)
{
    public static explicit operator GetVisitResponse(Visit visit)
    {
        return new GetVisitResponse(
            visit.Id,
            visit.WorkOrderId,
            visit.AssigneeFullName,
            visit.AssignedFrom,
            visit.Parts.Select(p => (GetPartResponse)p));
    }
}
public record UpdateVisitResponse();
public record CreateVisitResponse(Guid Id, Guid workOrderId, string assigneeFullName, DateTime assignedFrom, int totalParts, ICollection<CreatePartWithinVisitResponse> parts)
{
    public static explicit operator CreateVisitResponse(Visit visit)
    {
        return new CreateVisitResponse(
            visit.Id,
            visit.WorkOrderId,
            visit.AssigneeFullName,
            visit.AssignedFrom,
            visit.TotalParts,
            visit.Parts.Select(p => (CreatePartWithinVisitResponse)p).ToList());
    }
}
#endregion


