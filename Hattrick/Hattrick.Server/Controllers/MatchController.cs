using Hattrick.Server.Controllers.Interfaces;
using Hattrick.Server.HelperMethods;
using Hattrick.ServiceLayer.Models;
using Hattrick.Server.Requests;
using Hattrick.Server.Responses;
using Hattrick.ServiceLayer.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hattrick.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : Controller, IMatchController
    {
        private readonly IBetType _betTypesService;
        private readonly ICoefficient _coefficientService;
        private readonly IMatch _matchService;
        private readonly ISports _sportsService;
        private readonly ITopOffers _topOffersService;
        private readonly IUser _userService;
        private readonly ITicketService _ticketService;
        private readonly IWalletTransactionService _walletTransactionService;

        public MatchController(IBetType betTypesService, ICoefficient coefficientService, IMatch matchService,
            ISports sportsService, ITopOffers topOffersService, IUser userService, IWalletTransactionService walletTransaction, ITicketService ticketService)
        {
            _sportsService = sportsService;
            _topOffersService = topOffersService;
            _betTypesService = betTypesService;
            _coefficientService = coefficientService;
            _matchService = matchService;
            _userService = userService;
            _ticketService = ticketService;
            _walletTransactionService = walletTransaction;
        }

        [HttpGet]
        [Route("get")]
        public IEnumerable<MatchResponse> Index()
        {
            var response = new List<MatchResponse>();
            var coefficients = _coefficientService.GetAll();
            var matches = _matchService.GetAll();
            var betTypes = _betTypesService.GetAll();
            var sports = _sportsService.GetAll();
            var topOfferModels = _topOffersService.GetAll();

            foreach (var coefficient in coefficients)
            {
                coefficient.BetType = betTypes.First(x => x.Id == coefficient.BetTypeId);
            }

            foreach (var match in matches)
            {
                MatchResponse toAdd = Helper.MapProperties(coefficients, sports, topOfferModels, match);
                response.Add(toAdd);
            }

            return response;
        }

        [HttpPost]
        [Route("post")]
        public IActionResult Post([FromBody] MatchRequest matchRequest)
        {
            if (matchRequest.SelectedOdds == null || !matchRequest.SelectedOdds.Any())
                return Helper.Result(HttpStatusCode.BadRequest, "No odds selected.");

            double totalOdds = 1;
            foreach (var value in matchRequest.SelectedOdds)
                totalOdds *= value.Odd;

            var removeFromWallet = matchRequest.BetAmount;
            var afterTaxValue = removeFromWallet * 0.95;
            var potentialWin = afterTaxValue * totalOdds;
            var user = _userService.GetAll().First();

            if (user.WalletBalance < (decimal)removeFromWallet)
                return Helper.Result(HttpStatusCode.BadRequest, "You can not bet more than your wallet balance. Go to the user profile and add funds.");

            if (matchRequest.SelectedOdds.Count(x => x.IsTopOffer) > 1)
                return Helper.Result(HttpStatusCode.BadRequest, "You cannot select multiple top offer matches.");

            if (matchRequest.SelectedOdds.Count(x => x.IsTopOffer) == 1 &&
                matchRequest.SelectedOdds.Count(x => x.Odd > 1.1 && !x.IsTopOffer) < 5)
                return Helper.Result(HttpStatusCode.BadRequest, "You selected a top offer. You need 5 or more non-top offer pairs with odds greater than 1.1.");

            Helper.RemoveFromWallet(removeFromWallet, _userService, _walletTransactionService);

            TicketModel ticket = Helper.CreateTicket(totalOdds, removeFromWallet, potentialWin, user);

            var rnd = new Random();
            Helper.CalculateChancesOfWinning(ticket, rnd);
            _ticketService.AddTicket(ticket);

            if (ticket.DidBetWin)
                Helper.AddtoWallet(potentialWin, _userService, _walletTransactionService);

            return Helper.Result(ticket.DidBetWin ? HttpStatusCode.OK : HttpStatusCode.UnprocessableContent, ticket.DidBetWin ? "Ticket played, you won." : "Ticket played, you lost.");
        }
    }
}