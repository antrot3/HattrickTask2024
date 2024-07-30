using Hattrick.Server.Models;
using Hattrick.Server.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Hattrick.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        public HattrickDbContext _context { get; set; }
        public UserController(HattrickDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get")]
        public ReturnUserResponse Index()
        {
            var user = new ReturnUserResponse()
            {
                User = _context.Users.First(),
                WalletTransactionModels = new List<WalletTransactionModel>()
            };

            var walletTransactionModels = _context.WalletTransactions.ToList();
            foreach (var walletTransactionModel in walletTransactionModels.Where(x => x.UserId == user.User.Id))
            {
                user.WalletTransactionModels.Add(walletTransactionModel);
            }

            user.TicketsPlaid = new List<TicektBets>();
            foreach (var ticketPlaid in _context.Tickets.Where(x => x.UserId == user.User.Id))
            {
                var ticketBet = new TicektBets()
                {
                    TotalOdd = ticketPlaid.TotalOdd,
                    PotentialWinning = ticketPlaid.PotentialWinning,
                    Stake = ticketPlaid.Stake,
                    IsBetPlayed = ticketPlaid.IsBetPlayed,
                    DidBetWin = ticketPlaid.DidBetWin
                };
                ticketBet.MatchesPlaid = new List<TicektBetsPlaid>();
                user.TicketsPlaid.Add(ticketBet);
            }

            return user;
        }

        [HttpPost]
        [Route("post")]
        public void Post([FromBody] decimal balanceToAdd)
        {
            var user = _context.Users.First();
            _context.WalletTransactions.Add(new WalletTransactionModel()
            {
                Amount = balanceToAdd,
                TransactionType = "Add to wallet",
                UserId = user.Id,
                Date = DateTime.Now
            });
            user.WalletBalance += balanceToAdd;
            _context.SaveChanges();
        }

        [HttpPost]
        [Route("DepositToBank")]
        public void DepositToBank([FromBody] decimal balanceToWithraw)
        {
            var user = _context.Users.First();
            if (user.WalletBalance > balanceToWithraw)
            {
                _context.WalletTransactions.Add(new WalletTransactionModel()
                {
                    Amount = balanceToWithraw,
                    TransactionType = "Deposit to bank",
                    UserId = user.Id,
                    Date = DateTime.Now
                });
                user.WalletBalance -= balanceToWithraw;
                _context.SaveChanges();
            }
        }
    }
}
