using Hattrick.ServiceLayer.Models;

namespace Hattrick.ServiceLayer.Service
{
    public interface IBetType
    {
        List<BetTypeModel> GetAll();
    }

    public class BetTypesService : IBetType
    {
        private readonly HattrickDbContext _context;

        public BetTypesService(HattrickDbContext context)
        {
            _context = context;
        }

        public List<BetTypeModel> GetAll()
        {
            return _context.BetTypes.ToList();
        }
    }
}
