using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hattrick.Server.Models
{
    public class TopOfferModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Match")]
        public int MatchId { get; set; }
        public decimal OddValue { get; set; }
        public MatchModel Match { get; set; }
        public string ConditionDescription { get; set; }

    }

}
