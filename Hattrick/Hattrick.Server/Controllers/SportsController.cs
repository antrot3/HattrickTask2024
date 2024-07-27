using Hattrick.Server.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hattrick.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SportsController : ControllerBase
    {
        public HattrickDbContext _context { get; set; }
        public SportsController(HattrickDbContext context)
        {
            _context = context;
        }

        // GET: api/<Sports>
        [HttpGet]
        public IEnumerable<Sport> Get()
        {
           return _context.Sports.ToList();
        }

        // GET api/<Sports>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Sports>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _context.Sports.Add(new Models.Sport { Name = value });
            _context.SaveChanges();
        }

        // PUT api/<Sports>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Sports>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
