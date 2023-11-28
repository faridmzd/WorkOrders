using Microsoft.EntityFrameworkCore;
using W.O.API.Data.Repositories.Abstract;
using W.O.API.Domain;

namespace W.O.API.Data.Repositories.Concrete
{
    public class WorkOrderRepository : BaseRepository<WorkOrder>, IWorkOrderRepository
    {
        public WorkOrderRepository(AppDBContext context) : base(context) {}

        public override async Task<List<WorkOrder>> GetAllAsync()
        {
            return await _entities.Include(x => x.Visits).ThenInclude(x => x.Parts).ToListAsync() ?? new List<WorkOrder>();
        }

        public override async Task<WorkOrder?> GetByIdAsync(Guid id)
        {
            return await _entities.Include(x => x.Visits).ThenInclude(x => x.Parts).FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}

