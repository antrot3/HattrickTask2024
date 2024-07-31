using Hattrick.ServiceLayer.Models;

namespace Hattrick.ServiceLayer.Service
{
    public interface IMatch
    {
        List<MatchModel> GetAll();
    }

    public class MatchService : IMatch
    {
        private readonly HattrickDbContext _context;

        public MatchService(HattrickDbContext context)
        {
            _context = context;
        }

        public List<MatchModel> GetAll()
        {
            return _context.Matches.ToList();
        }
    }
}
