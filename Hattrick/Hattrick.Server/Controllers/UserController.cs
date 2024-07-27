using Hattrick.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet(Name = "GetUsers")]
        [Route("get")]
        public IEnumerable<UserModel> Index()
        {
            return _context.Users.ToList();
        }

    }
}
