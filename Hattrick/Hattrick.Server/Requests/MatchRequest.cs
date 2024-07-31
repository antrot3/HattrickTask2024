namespace Hattrick.Server.Requests
{
    public class MatchRequest()
    {
        public double BetAmount { get; set; }
        public List<SelectedValues> SelectedOdds { get; set; }
    }

    public class SelectedValues()
    {
        public int Id { get; set; }
        public bool IsTopOffer { get; set; }
        public string OddType { get; set; }
        public double Odd { get; set; }
        public string TeamHome { get; set; }
        public string TeamAway { get; set; }
    }
}
