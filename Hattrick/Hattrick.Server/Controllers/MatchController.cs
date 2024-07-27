using Hattrick.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hattrick.Server.Controllers
{
    public class MatchResponse()
    {
        public string SportName { get; set; }
        public string TeamHome { get; set; }
        public string TeamAway { get; set; }

        public DateTime Date { get; set; }

        public decimal OddValue1 { get; set; }
        public decimal OddValue2 { get; set; }
        public decimal OddValuex { get; set; }
        public decimal OddValue1x { get; set; }
        public decimal OddValue2x { get; set; }
        public decimal OddValue12 { get; set; }
    }


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
            foreach (var coeficient in coeficients)
            {
                coeficient.BetType = betTypes.First(x => x.Id == coeficient.BetTypeId);
            }

            foreach (var match in matches)
            {
                var toAdd = new MatchResponse();
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
                response.Add(toAdd);
            }
            return response;
        }

    }
}
