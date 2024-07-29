using Hattrick.Server.Models;

namespace Hattrick.Server.Responses
{
    public class ReturnUserResponse
    {
        public UserModel User { get; set; }

        public List<WalletTransactionModel> WalletTransactionModels { get; set; }

        public List<TicektBets> TicketsPlaid { get; set; }
    }

    public class TicektBets()
    {
        public decimal TotalOdd { get; set; }
        public decimal Stake { get; set; }
        public decimal PotentialWinning { get; set; }
        public bool IsBetPlayed { get; set; }
        public bool DidBetWin { get; set; }
        public List<TicektBetsPlaid> MatchesPlaid { get; set; }
    }

    public class TicektBetsPlaid()
    {
        public decimal Odd { get; set; }
        public string TeamHome { get; set; }
        public string TeamAway { get; set; }
    }
}
