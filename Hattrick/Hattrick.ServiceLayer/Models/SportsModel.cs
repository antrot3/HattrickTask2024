using System.ComponentModel.DataAnnotations;

namespace Hattrick.ServiceLayer.Models
{
    public class SportModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

