﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hattrick.ServiceLayer.Models
{
    public class TicketModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public decimal TotalOdd { get; set; }
        public decimal Stake { get; set; }
        public decimal PotentialWinning { get; set; }
        public DateTime Date { get; set; }
        public bool IsBetPlayed { get; set; }
        public bool DidBetWin { get; set; }
    }
}
