using Hattrick.Server.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using Hattrick.Server.Models;

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
            var coeficients = _context.coeficient.ToList();
            var matches = _context.Matches.ToList();
            var betTypes = _context.BetTypes.ToList();
            var sports = _context.Sports.ToList();
            var topOfferModels = _context.TopOffers.ToList();
            var user = _context.Users.First();

            foreach (var coeficient in coeficients)
            {
                coeficient.BetType = betTypes.First(x => x.Id == coeficient.BetTypeId);
            }

            foreach (var match in matches)
            {
                var toAdd = new MatchResponse();
                toAdd.Id = match.Id;
                toAdd.TeamAway = match.TeamAway;
                toAdd.TeamHome = match.TeamHome;
                toAdd.Date = match.Date;
                toAdd.SportName = sports.First(x => x.Id == match.SportId).Name;
                toAdd.OddValue1 = coeficients.Where(x => x.MatchId == match.Id && x.BetType.Name == "1").Select(x => x.OddValue).FirstOrDefault(1);
                toAdd.OddValue2 = coeficients.Where(x => x.MatchId == match.Id && x.BetType.Name == "2").Select(x => x.OddValue).FirstOrDefault(1);
                toAdd.OddValuex = coeficients.Where(x => x.MatchId == match.Id && x.BetType.Name == "X").Select(x => x.OddValue).FirstOrDefault(1);
                toAdd.OddValue1x = coeficients.Where(x => x.MatchId == match.Id && x.BetType.Name == "1X").Select(x => x.OddValue).FirstOrDefault(1);
                toAdd.OddValue2x = coeficients.Where(x => x.MatchId == match.Id && x.BetType.Name == "X2").Select(x => x.OddValue).FirstOrDefault(1);
                toAdd.OddValue12 = coeficients.Where(x => x.MatchId == match.Id && x.BetType.Name == "12").Select(x => x.OddValue).FirstOrDefault(1);
                toAdd.TopDescription = topOfferModels.Where(x => x.MatchId == match.Id).Select(x => x.ConditionDescription).FirstOrDefault("Does Not Exist");
                toAdd.OddValue = topOfferModels.Where(x => x.MatchId == match.Id).Select(x => x.OddValue).FirstOrDefault(1);
                response.Add(toAdd);
            }
            return response;
        }

        [HttpPost("post")]
        [Route("post")]
        public IActionResult Post([FromBody] MatchRequst matchRequst)
        {
            var removeFromWallet = matchRequst.BetAmmount;
            var afterTaxValue = removeFromWallet * 0.95;

            if (matchRequst.SelectedOdds == null || matchRequst.SelectedOdds.Count == 0)
            {
                return BadRequest("No odds selected");
            }
            double totalOds = 1;
            foreach (var value in matchRequst.SelectedOdds)
                totalOds = totalOds * value.Odd;
            var potentialWin = afterTaxValue * totalOds;

            if (_context.Users.First().WalletBalance < (decimal)removeFromWallet)
                return BadRequest("You can not bet more then your wallet ballance, Go to user Profile and add founs");
            if (matchRequst.SelectedOdds.Where(x => x.HasTopOffer == true).Count() > 1)
                return BadRequest("You can not select multiple top offer matches");

            if (matchRequst.SelectedOdds.Where(x => x.HasTopOffer == true).Count() == 1)
            {
                if (matchRequst.SelectedOdds.Where(x => x.Odd > 1.1).Count() <= 5)
                    return BadRequest("You selected top offer you need 5 or more non top offer pairs with odds grater then 1.1");
            }

            var user = _context.Users.FirstOrDefault();
            _context.WalletTransactions.Add(new WalletTransactionModel()
            {
                Amount = (decimal)removeFromWallet,
                TransactionType = "Bet",
                UserId = _context.Users.First().Id,
                Date = DateTime.Now
            });

            user.WalletBalance -= (decimal)removeFromWallet;

            var model = new TicketModel()
            {
                Stake = (decimal)removeFromWallet,
                Date = DateTime.Now,
                TotalOdd = (decimal)totalOds,
                UserId = _context.Users.First().Id,
                IsBetPlayed = true,
                PotentialWinning = (decimal)potentialWin,
                DidBetWin = false,
            };

            Random rnd = new Random();
            CalculateChancesOfWinning(model, rnd);
            _context.Tickets.Add(model);

            if (model.DidBetWin)
            {
                _context.WalletTransactions.Add(new WalletTransactionModel()
                {
                    Amount = (decimal)potentialWin,
                    TransactionType = "Bet Won",
                    UserId = _context.Users.First().Id,
                    Date = DateTime.Now
                });
                user.WalletBalance += (decimal)potentialWin;
            }

            _context.SaveChanges();

            if (model.DidBetWin)
                return Ok("Ticket is played, you won");
            else
                return Ok("Ticket is played, you lost");
        }

        //For easier testing i set chances to be 50% 50%
        private void CalculateChancesOfWinning(TicketModel model, Random rnd)
        {
            if (rnd.Next(1, 3) == 1)
                model.DidBetWin = true;
        }
    }
    public class MatchRequst()
    {
        public double BetAmmount { get; set; }
        public List<SelectedValues> SelectedOdds { get; set; }
    }

    public class SelectedValues()
    {
        public int Id { get; set; }
        public bool HasTopOffer { get; set; }
        public string OddType { get; set; }
        public double Odd { get; set; }
        public string TeamHome { get; set; }
        public string TeamAway { get; set; }
    }
}
