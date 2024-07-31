using Hattrick.ServiceLayer.Models;
using Hattrick.Server.Responses;
using Hattrick.ServiceLayer.Service;
using Microsoft.AspNetCore.Mvc;

namespace Hattrick.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IBetType _betTypesService;
        private readonly ICoefficient _coefficientService;
        private readonly IMatch _matchService;
        private readonly ISports _sportsService;
        private readonly ITopOffers _topOffersService;
        private readonly IUser _userService;
        private readonly ITicketService _ticketService;
        private readonly IWalletTransactionService _walletTransactionService;

        public UserController(IBetType betTypesService, ICoefficient coefficientService, IMatch matchService,
            ISports sportsService, ITopOffers topOffersService, IUser userService, ITicketService ticketService, IWalletTransactionService walletTransaction)
        {
            _betTypesService = betTypesService;
            _coefficientService = coefficientService;
            _matchService = matchService;
            _sportsService = sportsService;
            _topOffersService = topOffersService;
            _userService = userService;
            _ticketService = ticketService;
            _walletTransactionService = walletTransaction;
        }

        [HttpGet]
        [Route("get")]
        public ReturnUserResponse Index()
        {
            var user = new ReturnUserResponse()
            {
                User = _userService.GetAll().First(),
                WalletTransactionModels = new List<WalletTransactionModel>()
            };

            var walletTransactionModels = _walletTransactionService.GetAll().ToList();
            foreach (var walletTransactionModel in walletTransactionModels.Where(x => x.UserId == user.User.Id))
            {
                user.WalletTransactionModels.Add(walletTransactionModel);
            }

            user.TicketsPlaid = new List<TicektBets>();
            foreach (var ticketPlaid in _ticketService.GetAll().Where(x => x.UserId == user.User.Id))
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
            var user = _userService.GetAll().First();
            _walletTransactionService.AddTransaction(new WalletTransactionModel()
            {
                Amount = balanceToAdd,
                TransactionType = "Add to wallet",
                UserId = user.Id,
                Date = DateTime.Now
            });
            _userService.AddBallance((double)balanceToAdd);
        }

        [HttpPost]
        [Route("DepositToBank")]
        public void DepositToBank([FromBody] decimal balanceToWithraw)
        {
            var user = _userService.GetAll().First();
            if (user.WalletBalance > balanceToWithraw)
            {
                _walletTransactionService.AddTransaction(new WalletTransactionModel()
                {
                    Amount = balanceToWithraw,
                    TransactionType = "Deposit to bank",
                    UserId = user.Id,
                    Date = DateTime.Now
                });
                _userService.RemoveBallance((double)balanceToWithraw);
            }
        }
    }
}
