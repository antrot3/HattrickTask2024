using Hattrick.Server.Models;

namespace Hattrick.Server.Service
{
    public interface ICoefficient
    {
        List<CoefficientModel> GetAll();
    }

    public class CoefficientService : ICoefficient
    {
        private readonly HattrickDbContext _context;

        public CoefficientService(HattrickDbContext context)
        {
            _context = context;
        }

        public List<CoefficientModel> GetAll()
        {
            return _context.Coefficient.ToList();
        }
    }
}
