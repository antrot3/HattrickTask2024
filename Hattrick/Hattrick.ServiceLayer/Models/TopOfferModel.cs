using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hattrick.ServiceLayer.Models
{
    public class TopOfferModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Match")]
        public int MatchId { get; set; }
        public MatchModel Match { get; set; }
        public decimal OddMultiplier { get; set; }
    }

}
