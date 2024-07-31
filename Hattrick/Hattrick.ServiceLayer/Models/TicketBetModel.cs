using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hattrick.ServiceLayer.Models
{
    public class TicketBetModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public TicketModel Ticket { get; set; }
        [ForeignKey("Match")]
        public int MatchId { get; set; }
        public MatchModel Match { get; set; }
        [ForeignKey("BetType")]
        public int BetTypeId { get; set; }
        public BetTypeModel BetType { get; set; }
        public decimal Odd { get; set; }
    }
}
