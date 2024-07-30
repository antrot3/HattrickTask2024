using Hattrick.Server.Models;
using Hattrick.Server.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Hattrick.Server.HelperMethods
{
    public class Helper
    {
        public static void AddtoWallet(double potentialWin, UserModel user, HattrickDbContext context)
        {
            context.WalletTransactions.Add(new WalletTransactionModel
            {
                Amount = (decimal)potentialWin,
                TransactionType = "Bet Won",
                UserId = user.Id,
                Date = DateTime.Now
            });
            user.WalletBalance += (decimal)potentialWin;
        }
        public static TicketModel CreateTicket(double totalOdds, double removeFromWallet, double potentialWin, UserModel user)
        {
            return new TicketModel
            {
                Stake = (decimal)removeFromWallet,
                Date = DateTime.Now,
                TotalOdd = (decimal)totalOdds,
                UserId = user.Id,
                IsBetPlayed = true,
                PotentialWinning = (decimal)potentialWin,
                DidBetWin = false
            };
        }
        public static void RemoveFromWallet(double removeFromWallet, UserModel user, HattrickDbContext context)
        {
            context.WalletTransactions.Add(new WalletTransactionModel
            {
                Amount = (decimal)removeFromWallet,
                TransactionType = "Bet",
                UserId = user.Id,
                Date = DateTime.Now
            });

            user.WalletBalance -= (decimal)removeFromWallet;
        }
        // For easier testing, we set chances to be 50% 50%
        public static void CalculateChancesOfWinning(TicketModel model, Random rnd)
        {
            model.DidBetWin = rnd.Next(1, 3) == 2;
        }
        public static IActionResult Result(HttpStatusCode statusCode, string reason) => new ContentResult
        {
            StatusCode = (int)statusCode,
            Content = $"Status Code: {(int)statusCode}; {statusCode}; {reason}",
            ContentType = "text/plain"
        };
        public static MatchResponse MapProperties(List<CoefficientModel> coefficients, List<SportModel> sports, List<TopOfferModel> topOfferModels, MatchModel match)
        {
            return new MatchResponse
            {
                Id = match.Id,
                TeamAway = match.TeamAway,
                TeamHome = match.TeamHome,
                Date = match.Date,
                SportName = sports.First(x => x.Id == match.SportId).Name,
                OddValue1 = coefficients.FirstOrDefault(x => x.MatchId == match.Id && x.BetType.Name == "1")?.OddValue ?? 1,
                OddValue2 = coefficients.FirstOrDefault(x => x.MatchId == match.Id && x.BetType.Name == "2")?.OddValue ?? 1,
                OddValuex = coefficients.FirstOrDefault(x => x.MatchId == match.Id && x.BetType.Name == "X")?.OddValue ?? 1,
                OddValue1x = coefficients.FirstOrDefault(x => x.MatchId == match.Id && x.BetType.Name == "1X")?.OddValue ?? 1,
                OddValue2x = coefficients.FirstOrDefault(x => x.MatchId == match.Id && x.BetType.Name == "X2")?.OddValue ?? 1,
                OddValue12 = coefficients.FirstOrDefault(x => x.MatchId == match.Id && x.BetType.Name == "12")?.OddValue ?? 1,
                TopOfferMultiplier = topOfferModels.FirstOrDefault(x => x.MatchId == match.Id)?.OddMultiplier ?? 1
            };
        }
    }
}
