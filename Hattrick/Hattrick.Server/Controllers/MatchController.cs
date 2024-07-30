using Hattrick.Server.Controllers.Interfaces;
using Hattrick.Server.HelperMethods;
using Hattrick.Server.Models;
using Hattrick.Server.Requests;
using Hattrick.Server.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hattrick.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : Controller, IMatchController
    {
        public HattrickDbContext _context { get; set; }

        public MatchController(HattrickDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("get")]
        public IEnumerable<MatchResponse> Index()
        {
            var response = new List<MatchResponse>();
            var coefficients = _context.Coefficient.ToList();
            var matches = _context.Matches.ToList();
            var betTypes = _context.BetTypes.ToList();
            var sports = _context.Sports.ToList();
            var topOfferModels = _context.TopOffers.ToList();

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
            var user = _context.Users.First();

            if (user.WalletBalance < (decimal)removeFromWallet)
                return Helper.Result(HttpStatusCode.BadRequest, "You can not bet more than your wallet balance. Go to the user profile and add funds.");

            if (matchRequest.SelectedOdds.Count(x => x.IsTopOffer) > 1)
                return Helper.Result(HttpStatusCode.BadRequest, "You cannot select multiple top offer matches.");

            if (matchRequest.SelectedOdds.Count(x => x.IsTopOffer) == 1 &&
                matchRequest.SelectedOdds.Count(x => x.Odd > 1.1 && !x.IsTopOffer) < 5)
                return Helper.Result(HttpStatusCode.BadRequest, "You selected a top offer. You need 5 or more non-top offer pairs with odds greater than 1.1.");

            Helper.RemoveFromWallet(removeFromWallet, user, _context);

            TicketModel ticket = Helper.CreateTicket(totalOdds, removeFromWallet, potentialWin, user);

            var rnd = new Random();
            Helper.CalculateChancesOfWinning(ticket, rnd);
            _context.Tickets.Add(ticket);

            if (ticket.DidBetWin)
                Helper.AddtoWallet(potentialWin, user, _context);

            _context.SaveChanges();

            return Helper.Result(ticket.DidBetWin ? HttpStatusCode.OK : HttpStatusCode.UnprocessableContent, ticket.DidBetWin ? "Ticket played, you won." : "Ticket played, you lost.");
        }
    }
}