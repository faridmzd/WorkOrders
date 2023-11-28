using Microsoft.EntityFrameworkCore;
using W.O.API.Data.Repositories.Abstract;
using W.O.API.Domain.Common;

namespace W.O.API.Data.Repositories.Concrete
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Base
    {
        protected readonly AppDBContext _dbContext;
        protected readonly DbSet<T> _entities;
        public BaseRepository(AppDBContext context)
        {
            _dbContext = context;
            _entities = context.Set<T>();
        }
        public virtual async Task<T> AddAsync(T entity)
        {
            _entities.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<int> DeleteAsync(Guid id)
        {
            return await _entities.Where(e => e.Id == id).ExecuteDeleteAsync();

        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return (await _entities.ToListAsync()) ?? new List<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
