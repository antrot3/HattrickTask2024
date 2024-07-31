using Hattrick.ServiceLayer.Models;

namespace Hattrick.ServiceLayer.Service
{
    public interface ITicketService
    {
        List<TicketModel> GetAll();
        public void AddTicket(TicketModel ticket);
    }

    public class TicketService : ITicketService
    {
        private readonly HattrickDbContext _context;

        public TicketService(HattrickDbContext context)
        {
            _context = context;
        }

        public List<TicketModel> GetAll()
        {
            return _context.Tickets.ToList();
        }

        public void AddTicket(TicketModel ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }
    }
}
