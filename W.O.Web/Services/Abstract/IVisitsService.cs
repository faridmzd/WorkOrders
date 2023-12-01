using FluentResults;
using W.O.Web.Models;
using W.O.Web.Models.Requests.Create;

namespace W.O.Web.Services.Abstract
{
    public interface IVisitsService
    {
        Task<Result<IEnumerable<VisitDTO>>> GetAllAsync();
        Task<Result<VisitDTO>> GetByIdAsync(Guid id);
        Task<Result<VisitDTO>> AddAsync(CreateVisitRequest entity);
        Task<Result> UpdateAsync(VisitDTO entity);
        Task<Result> DeleteAsync(Guid id);
    }
}
