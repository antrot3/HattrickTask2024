using System.ComponentModel.DataAnnotations;

namespace Hattrick.ServiceLayer.Models
{
    public class BetTypeModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
