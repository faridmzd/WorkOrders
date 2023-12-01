using FluentResults;
using W.O.Web.Models;
using W.O.Web.Models.Requests.Create;

namespace W.O.Web.Services.Abstract
{
    public interface IPartsService
    {
        Task<Result<IEnumerable<PartDTO>>> GetAllAsync();
        Task<Result<PartDTO>> GetByIdAsync(Guid id);
        Task<Result<PartDTO>> AddAsync(CreatePartRequest entity);
        Task<Result> UpdateAsync(PartDTO entity);
        Task<Result> DeleteAsync(Guid id);
    }
}
