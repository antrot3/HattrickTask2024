using Hattrick.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Hattrick.Server
{
    public class HattrickDbContext : DbContext
    {
        public HattrickDbContext(DbContextOptions<HattrickDbContext> options) : base(options)
        {
        }
        public DbSet<SportModel> Sports { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<MatchModel> Matches { get; set; }
        public DbSet<BetTypeModel> BetTypes { get; set; }
        public DbSet<CoefficientModel> Coefficient { get; set; }
        public DbSet<TopOfferModel> TopOffers { get; set; }
        public DbSet<WalletTransactionModel> WalletTransactions { get; set; }
        public DbSet<TicketModel> Tickets { get; set; }
        public DbSet<TicketBetModel> TicketBets { get; set; }
    }
}
