using W.O.API.Data.Repositories.Abstract;
using W.O.API.Domain;

namespace W.O.API.Data.Repositories.Concrete
{
    public class PartRepository : BaseRepository<Part>, IPartRepository
    {
        public PartRepository(AppDBContext context) : base(context)
        {

        }
    }
}

