using Microsoft.EntityFrameworkCore;
using W.O.API.Data.Repositories.Abstract;
using W.O.API.Domain;

namespace W.O.API.Data.Repositories.Concrete
{
    public class VisitRepository : BaseRepository<Visit>, IVisitRepository
    {
        public VisitRepository(AppDBContext context) : base(context) {}

        public override async Task<List<Visit>> GetAllAsync()
        {
           return await _entities.Include(v => v.Parts).ToListAsync() ?? new List<Visit>();
        }

        public override async Task<Visit?> GetByIdAsync(Guid id)
        {
            return await _entities.Include(x => x.Parts).FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}

