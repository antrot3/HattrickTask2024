using Hattrick.Server.Models;
using Hattrick.Server.Requests;
using Hattrick.Server.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Hattrick.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : Controller
    {
        public HattrickDbContext _context { get; set; }

        public MatchController(HattrickDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
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
                var toAdd = new MatchResponse
                {
                    Id = match.Id,
                    TeamAway = match.TeamAway,
                    TeamHome = match.TeamHome,
                    Date = match.Date,
                    SportName = sports.First(x => x.Id == match.SportId).Name,
                    OddValue1 = coefficients.FirstOrDefault(x => x.MatchId == match.Id && x.BetType.Name == "1")?.OddValue ?? 1,
                    OddValue2 = coefficients.FirstOrDefault(x => x.MatchId == match.Id && x.BetType.Name == "2")?.OddValue ?? 1,
                    OddValuex = coefficients.FirstOrDefault(x => x.MatchId == match.Id && x.BetType.Name == "X")?.OddValue ?? 1,
                    OddValue1x = coefficients.FirstOrDefault(x => x.MatchId == match.Id && x.BetType.Name == "1X")?.OddValue ?? 1,
                    OddValue2x = coefficients.FirstOrDefault(x => x.MatchId == match.Id && x.BetType.Name == "X2")?.OddValue ?? 1,
                    OddValue12 = coefficients.FirstOrDefault(x => x.MatchId == match.Id && x.BetType.Name == "12")?.OddValue ?? 1,
                    TopOfferMultiplier = topOfferModels.FirstOrDefault(x => x.MatchId == match.Id)?.OddMultiplier ?? 1
                };

                response.Add(toAdd);
            }

            return response;
        }

        [HttpPost("post")]
        public IActionResult Post([FromBody] MatchRequest matchRequest)
        {
            var removeFromWallet = matchRequest.BetAmount;
            var afterTaxValue = removeFromWallet * 0.95;

            if (matchRequest.SelectedOdds == null || !matchRequest.SelectedOdds.Any())
            {
                return Result(HttpStatusCode.BadRequest, "No odds selected.");
            }

            double totalOdds = 1;
            foreach (var value in matchRequest.SelectedOdds)
            {
                totalOdds *= value.Odd;
            }

            var potentialWin = afterTaxValue * totalOdds;
            var user = _context.Users.First();

            if (user.WalletBalance < (decimal)removeFromWallet)
            {
                return Result(HttpStatusCode.BadRequest, "You can not bet more than your wallet balance. Go to the user profile and add funds.");
            }

            if (matchRequest.SelectedOdds.Count(x => x.IsTopOffer) > 1)
            {
                return Result(HttpStatusCode.BadRequest, "You cannot select multiple top offer matches.");
            }

            if (matchRequest.SelectedOdds.Count(x => x.IsTopOffer) == 1 &&
                matchRequest.SelectedOdds.Count(x => x.Odd > 1.1 && !x.IsTopOffer) < 5)
            {
                return Result(HttpStatusCode.BadRequest, "You selected a top offer. You need 5 or more non-top offer pairs with odds greater than 1.1.");
            }

            _context.WalletTransactions.Add(new WalletTransactionModel
            {
                Amount = (decimal)removeFromWallet,
                TransactionType = "Bet",
                UserId = user.Id,
                Date = DateTime.Now
            });

            user.WalletBalance -= (decimal)removeFromWallet;

            var ticket = new TicketModel
            {
                Stake = (decimal)removeFromWallet,
                Date = DateTime.Now,
                TotalOdd = (decimal)totalOdds,
                UserId = user.Id,
                IsBetPlayed = true,
                PotentialWinning = (decimal)potentialWin,
                DidBetWin = false
            };

            var rnd = new Random();
            CalculateChancesOfWinning(ticket, rnd);
            _context.Tickets.Add(ticket);

            if (ticket.DidBetWin)
            {
                _context.WalletTransactions.Add(new WalletTransactionModel
                {
                    Amount = (decimal)potentialWin,
                    TransactionType = "Bet Won",
                    UserId = user.Id,
                    Date = DateTime.Now
                });
                user.WalletBalance += (decimal)potentialWin;
            }

            _context.SaveChanges();

            return Result(ticket.DidBetWin ? HttpStatusCode.OK : HttpStatusCode.UnprocessableContent, ticket.DidBetWin ? "Ticket played, you won." : "Ticket played, you lost.");
        }

        private static IActionResult Result(HttpStatusCode statusCode, string reason) => new ContentResult
        {
            StatusCode = (int)statusCode,
            Content = $"Status Code: {(int)statusCode}; {statusCode}; {reason}",
            ContentType = "text/plain"
        };

        // For easier testing, we set chances to be 50% 50%
        private void CalculateChancesOfWinning(TicketModel model, Random rnd)
        {
            model.DidBetWin = rnd.Next(1, 3) == 2;
        }
    }
}