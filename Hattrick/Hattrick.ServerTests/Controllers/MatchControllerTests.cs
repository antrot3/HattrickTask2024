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
using Hattrick.Server.Service;
using Hattrick.Server.Responses;

namespace Hattrick.Server.Controllers.Tests
{


    [TestClass]
    public class MatchControllerTests
    {
        private readonly Mock<IBetType> _mockBetTypesService;
        private readonly Mock<ICoefficient> _mockCoefficientService;
        private readonly Mock<IMatch> _mockMatchService;
        private readonly Mock<ISports> _mockSportsService;
        private readonly Mock<ITopOffers> _mockTopOffersService;
        private readonly Mock<IUser> _mockUserService;
        private readonly Mock<ITicketService> _mockTicketService;
        private readonly Mock<IWalletTransactionService> _mockWalletTransactionService;
        private readonly MatchController _controller;

        public MatchControllerTests()
        {
            _mockBetTypesService = new Mock<IBetType>();
            _mockCoefficientService = new Mock<ICoefficient>();
            _mockMatchService = new Mock<IMatch>();
            _mockSportsService = new Mock<ISports>();
            _mockTopOffersService = new Mock<ITopOffers>();
            _mockUserService = new Mock<IUser>();
            _mockTicketService = new Mock<ITicketService>();
            _mockWalletTransactionService = new Mock<IWalletTransactionService>();

            _controller = new MatchController(
                _mockBetTypesService.Object,
                _mockCoefficientService.Object,
                _mockMatchService.Object,
                _mockSportsService.Object,
                _mockTopOffersService.Object,
                _mockUserService.Object,
                _mockWalletTransactionService.Object,
                _mockTicketService.Object
            );
        }

        [TestMethod]
        [TestProperty("Task", "IndexTest")]
        public void Index_ReturnsCorrectAmmountOfData()
        {
            // Arrange
            var coefficients = new List<CoefficientModel>
            {
                new CoefficientModel { Id = 1, MatchId = 1, BetTypeId = 1, OddValue = 1.5m },
            };
            var matches = new List<MatchModel>
            {
                new MatchModel { Id = 1, TeamAway = "Team A", TeamHome = "Team B", Date = DateTime.Now, SportId = 1 },
            };
            var betTypes = new List<BetTypeModel>
            {
                new BetTypeModel { Id = 1, Name = "1" },
            };
            var sports = new List<SportModel>
            {
                new SportModel { Id = 1, Name = "Football" },
            };
            var topOffers = new List<TopOfferModel>
            {
                new TopOfferModel { MatchId = 1, OddMultiplier = 2.45m },
            };

            _mockCoefficientService.Setup(service => service.GetAll()).Returns(coefficients);
            _mockMatchService.Setup(service => service.GetAll()).Returns(matches);
            _mockBetTypesService.Setup(service => service.GetAll()).Returns(betTypes);
            _mockSportsService.Setup(service => service.GetAll()).Returns(sports);
            _mockTopOffersService.Setup(service => service.GetAll()).Returns(topOffers);

            // Act
            var result = _controller.Index();

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        [TestProperty("Task", "IndexTest")]
        public void Index_ReturnsCorrectData()
        {
            // Arrange
            var coefficients = new List<CoefficientModel>
            {
                new CoefficientModel { Id = 1, MatchId = 1, BetTypeId = 1, OddValue = 1.5m },
                new CoefficientModel { Id = 2, MatchId = 1, BetTypeId = 2, OddValue = 2.5m }
            };
            var matches = new List<MatchModel>
            {
                new MatchModel { Id = 1, TeamAway = "Team A", TeamHome = "Team B", Date = DateTime.Now, SportId = 1 }
            };
            var betTypes = new List<BetTypeModel>
            {
                new BetTypeModel { Id = 1, Name = "1" },
                new BetTypeModel { Id = 2, Name = "2" }
            };
            var sports = new List<SportModel>
            {
                new SportModel { Id = 1, Name = "Football" }
            };
            var topOffers = new List<TopOfferModel>
            {
                new TopOfferModel { MatchId = 1, OddMultiplier = 2.0m }
            };

            _mockCoefficientService.Setup(service => service.GetAll()).Returns(coefficients);
            _mockMatchService.Setup(service => service.GetAll()).Returns(matches);
            _mockBetTypesService.Setup(service => service.GetAll()).Returns(betTypes);
            _mockSportsService.Setup(service => service.GetAll()).Returns(sports);
            _mockTopOffersService.Setup(service => service.GetAll()).Returns(topOffers);

            // Act
            var result = _controller.Index().ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Team A", result[0].TeamAway);
            Assert.AreEqual("Team B", result[0].TeamHome);
            Assert.AreEqual("Football", result[0].SportName);
            Assert.AreEqual(1.5m, result[0].OddValue1);
            Assert.AreEqual(2.5m, result[0].OddValue2);
            Assert.AreEqual(2.0m, result[0].TopOfferMultiplier);
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