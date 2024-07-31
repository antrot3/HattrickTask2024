using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hattrick.ServiceLayer.Models
{
    public class MatchModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Sport")]
        public int SportId { get; set; }
        public SportModel Sport { get; set; }
        public string TeamHome { get; set; }
        public string TeamAway { get; set; }
        public DateTime Date { get; set; }
    }

}
