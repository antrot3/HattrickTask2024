using Hattrick.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Hattrick.Server.Service
{
    public interface IMatch
    {
        List<MatchModel> GetAll();
    }

    public class MatchService : IMatch
    {
        private readonly HattrickDbContext _context;

        public MatchService(HattrickDbContext context)
        {
            _context = context;
        }

        //public void DeleteCategory(int id)
        //{
        //    var category = GetById(id);
        //    if (category == null)
        //    {
        //        throw new ArgumentException();
        //    }
        //    _context.Remove(category);
        //    _context.SaveChanges();
        //}

        //public void EditCategory(Category category)
        //{
        //    _context.Update(category);
        //    _context.SaveChanges();
        //}

        public List<MatchModel> GetAll()
        {
            return _context.Matches.ToList();
        }

        //public Category GetById(int id)
        //{
        //    return GetAll().FirstOrDefault(category => category.Id == id);
        //}

        //public void NewCategory(Category category)
        //{
        //    _context.Add(category);
        //    _context.SaveChanges();
        //}
    }
}
