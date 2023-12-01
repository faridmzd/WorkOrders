using FluentResults;
using W.O.Web.Models;
using W.O.Web.Models.Requests.Create;
using W.O.Web.Pages;

namespace W.O.Web.Services.Abstract
{
    public interface IWorkOrdersService
    {
        Task<Result<IEnumerable<WorkOrderDTO>>> GetAllAsync();
        Task<Result<WorkOrderDetailsDTO>> GetByIdAsync(Guid id);
		Task<Result<WorkOrderDTO>> AddAsync(CreateWorkOrderRequest entity);
        Task<Result> UpdateAsync(WorkOrderDTO entity);
        Task<Result> DeleteAsync(Guid id);
    }
}
