using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hattrick.Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hattrick.Server.Requests;
using Moq.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore;
using Hattrick.Server.Models;
using Hattrick.ServerTests;
using MySqlConnector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Hattrick.Server.Controllers.Tests
{
    [TestClass]
    public class MatchControllerTests
    {
        [TestMethod]
        [TestProperty("Task", "PostTest")]
        public void Post_Always_ReturnsCorrectResult()
        {
            //Arange
            var connection = new MySqlConnection("Server=localhost;User=root;Password=admin;Database=HattrickTestDatabase");
            var options = new DbContextOptionsBuilder<HattrickDbContext>()
                    .UseMySql(connection,
        new MySqlServerVersion(new Version(8, 0, 21)))
                    .Options;
            var context = new HattrickDbContext(options);
            context.Users.Add(TestDataHelper.GetFakeUserList().First());

            MatchController controller = new MatchController(context);
            var SelectedOdds = new List<SelectedValues>();
            AddToSelectedOdds(SelectedOdds, 1, true, 2, "1x", "Hajduk", "Dinamo");
            MatchRequest request = new MatchRequest() { BetAmount = 100, SelectedOdds = SelectedOdds };

            //Act 
            var response = controller.Post(request);
            var statusCode = ((IStatusCodeActionResult)response).StatusCode;

            //Assert
            Assert.AreEqual(statusCode, StatusCodes.Status400BadRequest);
        }

        public void AddToSelectedOdds(List<SelectedValues> SelectedOdds, int id, bool isTopOffer, int odd, string oddType, string homeTeam, string teamAway)
        {
            SelectedOdds.Add(new SelectedValues()
            {
                Id = id,
                IsTopOffer = isTopOffer,
                Odd = odd,
                TeamAway = teamAway,
                TeamHome = homeTeam,
                OddType = oddType
            });
        }
    }
}