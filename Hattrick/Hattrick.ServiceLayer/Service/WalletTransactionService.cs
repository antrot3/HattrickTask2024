using Hattrick.Server.Models;

namespace Hattrick.Server.Service
{
    public interface IWalletTransactionService
    {
        List<WalletTransactionModel> GetAll();
        void AddTransaction(WalletTransactionModel model);
    }

    public class WalletTransactionService : IWalletTransactionService
    {
        private readonly HattrickDbContext _context;

        public WalletTransactionService(HattrickDbContext context)
        {
            _context = context;
        }
        public List<WalletTransactionModel> GetAll()
        {
            return _context.WalletTransactions.ToList();
        }

        public void AddTransaction(WalletTransactionModel model)
        {
            _context.WalletTransactions.Add(model);
            _context.SaveChanges();
        }
    }
}
