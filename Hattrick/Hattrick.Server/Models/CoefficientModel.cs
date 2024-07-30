using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hattrick.Server.Models
{
    public class CoefficientModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Match")]
        public int MatchId { get; set; }
        public MatchModel Match { get; set; }
        [ForeignKey("BetType")]
        public int BetTypeId { get; set; }
        public BetTypeModel BetType { get; set; }
        public decimal OddValue { get; set; }
    }
}
