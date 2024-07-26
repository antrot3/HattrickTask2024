using System.ComponentModel.DataAnnotations;

namespace Hattrick.Server.Models
{
    public class Sport
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

