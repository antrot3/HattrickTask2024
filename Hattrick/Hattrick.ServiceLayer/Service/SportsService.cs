using Hattrick.Server.Models;

namespace Hattrick.Server.Service
{
    public interface ISports
    {
        List<SportModel> GetAll();
    }

    public class SportsService : ISports
    {
        private readonly HattrickDbContext _context;

        public SportsService(HattrickDbContext context)
        {
            _context = context;
        }

        public List<SportModel> GetAll()
        {
            return _context.Sports.ToList();
        }

    }
}
