using Hattrick.Server.Models;

namespace Hattrick.Server.Service
{
    public interface IUser
    {
        List<UserModel> GetAll();
        void AddBallance(double addBalance);
        void RemoveBallance(double removeBalance);
    }

    public class UserService : IUser
    {
        private readonly HattrickDbContext _context;

        public UserService(HattrickDbContext context)
        {
            _context = context;
        }

        public List<UserModel> GetAll()
        {
            return _context.Users.ToList();
        }

        public void AddBallance(double addBalance)
        {
            _context.Users.First().WalletBalance += (decimal)addBalance;
            _context.SaveChanges();
        }

        public void RemoveBallance(double removeBalance)
        {
            _context.Users.First().WalletBalance -= (decimal)removeBalance;
            _context.SaveChanges();
        }
    }
}
