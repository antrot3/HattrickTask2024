﻿namespace Hattrick.Server.Responses
{
    public class MatchResponse()
    {
        public string SportName { get; set; }
        public string TeamHome { get; set; }
        public string TeamAway { get; set; }

        public DateTime Date { get; set; }

        public decimal OddValue1 { get; set; }
        public decimal OddValue2 { get; set; }
        public decimal OddValuex { get; set; }
        public decimal OddValue1x { get; set; }
        public decimal OddValue2x { get; set; }
        public decimal OddValue12 { get; set; }
    }
}
