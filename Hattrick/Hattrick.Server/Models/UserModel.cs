using System.ComponentModel.DataAnnotations;

namespace Hattrick.Server.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal WalletBalance { get; set; }
    }
}
