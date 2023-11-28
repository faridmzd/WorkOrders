using W.O.API.Domain.Common;

namespace W.O.API.Data.Repositories.Abstract
{
    public interface IBaseRepository<T> where T : Base
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<int> DeleteAsync(Guid id);
    }
}
