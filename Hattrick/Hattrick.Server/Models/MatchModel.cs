using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hattrick.Server.Models
{
    public class MatchModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Sport")]
        public int SportId { get; set; }
        public Sport Sport { get; set; }
        public string TeamHome { get; set; }
        public string TeamAway { get; set; }
        public DateTime Date { get; set; }
    }

}
