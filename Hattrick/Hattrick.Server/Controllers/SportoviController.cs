using Hattrick.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hattrick.Server.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class SportoviController : Controller
    {
        public HattrickDbContext _context { get; set; }
        public SportoviController(HattrickDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetSports")]
        [Route("get")]
        public IEnumerable<SportModel> Index()
        {
            return _context.Sports.ToList();
        }

        [HttpPost(Name = "AddSport")]
        [Route("post")]
        public void AddSport([FromBody] string name)
        {
            _context.Sports.Add(new SportModel() { Name = name });
            _context.SaveChanges();
        }

    }
}
