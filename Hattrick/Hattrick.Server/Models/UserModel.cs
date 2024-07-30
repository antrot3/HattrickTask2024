using System.ComponentModel.DataAnnotations;

namespace Hattrick.Server.Models
{
    public interface IUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal WalletBalance { get; set; }
    }
    public class UserModel: IUserModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal WalletBalance { get; set; }
    }
}
