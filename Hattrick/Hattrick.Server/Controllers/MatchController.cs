using Hattrick.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hattrick.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : Controller
    {
        public HattrickDbContext _context { get; set; }
        public IServiceProvider _serviceProvider { get; set; }
        public MatchController(HattrickDbContext context)
        {
            _context = context;
        }
        public void InitializeMatch(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetService(typeof(HattrickDbContext)) as HattrickDbContext;
        }

        [HttpGet(Name = "GetMatches")]
        public IEnumerable<MatchModel> Index()
        {
            var result = _context.Matches.ToList();
            var sports = _context.Sports.ToList();
            foreach (var mathc in result)
            {
                mathc.Sport = _context.Sports.First(x => x.Id == mathc.SportId);
            }
            return _context.Matches.ToList();
        }

        // GET: MatchController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MatchController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MatchController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MatchController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MatchController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MatchController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MatchController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
