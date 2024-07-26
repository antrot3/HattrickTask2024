using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hattrick.Server.Models
{
    public class WalletTransactionModel
    {

        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public DateTime Date { get; set; }
    }
}
