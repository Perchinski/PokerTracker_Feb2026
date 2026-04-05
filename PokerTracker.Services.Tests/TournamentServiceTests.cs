using Microsoft.EntityFrameworkCore;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using PokerTracker.Data.Models;
using PokerTracker.Data.Repository.Contracts;
using PokerTracker.Services.Core;
using PokerTracker.ViewModels.Tournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerTracker.Services.Tests
{
    [TestFixture]
    public class TournamentServiceTests
    {
        private Mock<ITournamentRepository> mockRepo;
        private TournamentService service;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<ITournamentRepository>();

            // Default common mock setups
            mockRepo.Setup(r => r.FormatExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            mockRepo.Setup(r => r.LocationExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            service = new TournamentService(mockRepo.Object);
        }

        [Test]
        public async Task GetFormatsAsync_ShouldReturnFormats()
        {
            // Arrange
            var formats = new List<TournamentFormat>
            {
                new TournamentFormat { Id = 1, Name = "Texas Hold'em" },
                new TournamentFormat { Id = 2, Name = "Omaha" }
            }.AsQueryable().BuildMock();

            mockRepo.Setup(r => r.GetAllFormatsQuery()).Returns(formats);

            // Act
            var result = await service.GetFormatsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Texas Hold'em"));
        }

        [Test]
        public async Task GetActiveLocationsAsync_ShouldReturnActiveLocations()
        {
            // Arrange
            var locations = new List<Location>
            {
                new Location { Id = 1, Name = "Casino Royale", City = "Las Vegas" },
                new Location { Id = 2, Name = "Monte Carlo", City = "Monaco" }
            }.AsQueryable().BuildMock();

            mockRepo.Setup(r => r.GetActiveLocationsQuery()).Returns(locations);

            // Act
            var result = await service.GetActiveLocationsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Casino Royale"));
        }

        [Test]
        public async Task CreateAsync_InvalidFormat_ThrowsArgumentException()
        {
            // Arrange
            var model = new TournamentFormModel { FormatId = 1, LocationId = 1 };
            mockRepo.Setup(r => r.FormatExistsAsync(1)).ReturnsAsync(false);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(model, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Invalid tournament format."));
        }

        [Test]
        public async Task CreateAsync_InvalidLocation_ThrowsArgumentException()
        {
            // Arrange
            var model = new TournamentFormModel { FormatId = 1, LocationId = 1 };
            mockRepo.Setup(r => r.LocationExistsAsync(1)).ReturnsAsync(false);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(model, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Invalid location."));
        }

        [Test]
        public async Task CreateAsync_ValidData_AddsTournamentAndSaves()
        {
            // Arrange
            var model = new TournamentFormModel { Name = "Big One", FormatId = 1, LocationId = 1, Description = "Test", ImageUrl = "", Date = DateTime.Now };

            // Act
            await service.CreateAsync(model, "user1");

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Tournament>()), Times.Once);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
        
        [Test]
        public async Task StartAsync_ValidData_UpdatesStatusToRunning()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Open, CreatorId = "user1" };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act
            await service.StartAsync(1, "user1");

            // Assert
            Assert.That(tournament.Status, Is.EqualTo(TournamentStatus.Running));
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task StartAsync_Unauthorized_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Open, CreatorId = "owner" };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.StartAsync(1, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Only the creator or an admin can start an open tournament."));
        }

        [Test]
        public async Task FinishAsync_ValidData_UpdatesStatusToFinished()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Running, CreatorId = "user1" };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act
            await service.FinishAsync(1, "user1");

            // Assert
            Assert.That(tournament.Status, Is.EqualTo(TournamentStatus.Finished));
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task JoinAsync_ValidData_AddsPlayer()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Open, PlayersTournaments = new List<PlayerTournament>() };
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(1)).ReturnsAsync(tournament);

            // Act
            await service.JoinAsync(1, "user1");

            // Assert
            Assert.That(tournament.PlayersTournaments.Count, Is.EqualTo(1));
            Assert.That(tournament.PlayersTournaments.First().PlayerId, Is.EqualTo("user1"));
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task JoinAsync_NotOpen_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Running, PlayersTournaments = new List<PlayerTournament>() };
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(1)).ReturnsAsync(tournament);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.JoinAsync(1, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Tournament is not open."));
        }

        [Test]
        public async Task LeaveAsync_ValidData_RemovesPlayer()
        {
            // Arrange
            var player = new PlayerTournament { TournamentId = 1, PlayerId = "user1" };
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Open, PlayersTournaments = new List<PlayerTournament> { player } };
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(1)).ReturnsAsync(tournament);

            // Act
            await service.LeaveAsync(1, "user1");

            // Assert
            Assert.That(tournament.PlayersTournaments.Count, Is.EqualTo(0));
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllTournamentsAsync_ReturnsPagedTournaments()
        {
            // Arrange
            var testUser = new Microsoft.AspNetCore.Identity.IdentityUser { Id = "user1", UserName = "tester" };
            var format = new TournamentFormat { Id = 1, Name = "Format" };
            var locations = new Location { Id = 1, Name = "Loc" };
            
            var query = new List<Tournament>
            {
                new Tournament 
                { 
                    Id = 1, 
                    Name = "T1", 
                    Description = "Desc",
                    Date = DateTime.Now,
                    FormatId = 1,
                    Format = format,
                    LocationId = 1,
                    Location = locations,
                    Status = TournamentStatus.Open,
                    CreatorId = "user1",
                    Creator = testUser,
                    PlayersTournaments = new List<PlayerTournament>()
                }
            }.AsQueryable().BuildMock();

            mockRepo.Setup(r => r.GetAllTournamentsQuery()).Returns(query);

            // Act
            var (tournaments, totalCount) = await service.GetAllTournamentsAsync(null, null, null, "DateAscending", false, false, "user1", false, 1, 10);

            // Assert
            Assert.That(totalCount, Is.EqualTo(1));
            Assert.That(tournaments.Count, Is.EqualTo(1));
            Assert.That(tournaments.First().Name, Is.EqualTo("T1"));
        }

        [Test]
        public async Task RemovePlayerAsync_ValidData_RemovesPlayer()
        {
            // Arrange
            var player = new PlayerTournament { TournamentId = 1, PlayerId = "userToRemove" };
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Open, CreatorId = "owner", PlayersTournaments = new List<PlayerTournament> { player } };
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(1)).ReturnsAsync(tournament);

            // Act
            await service.RemovePlayerAsync(1, "userToRemove", "owner");

            // Assert
            Assert.That(tournament.PlayersTournaments.Count, Is.EqualTo(0));
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsDetails_WhenFound()
        {
            // Arrange
            var format = new TournamentFormat { Id = 1, Name = "Format" };
            var testUser = new Microsoft.AspNetCore.Identity.IdentityUser { Id = "owner", UserName = "tester" };
            var tournament = new Tournament
            {
                Id = 1,
                Name = "Big One",
                Description = "Test",
                Date = DateTime.Now,
                FormatId = 1,
                Format = format,
                LocationId = 1,
                Status = TournamentStatus.Open,
                CreatorId = "owner",
                Creator = testUser,
                PlayersTournaments = new List<PlayerTournament>()
            };
            mockRepo.Setup(r => r.GetDetailsByIdAsync(1)).ReturnsAsync(tournament);

            // Act
            var result = await service.GetDetailsAsync(1, "user1");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Big One"));
            Assert.That(result.Format, Is.EqualTo("Format"));
            Assert.That(result.IsCreator, Is.False);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            mockRepo.Setup(r => r.GetDetailsByIdAsync(99)).ReturnsAsync((Tournament)null);

            // Act
            var result = await service.GetDetailsAsync(99, "user1");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetForEditAsync_ReturnsModel_WhenOwner()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Name = "T", CreatorId = "user1" };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act
            var result = await service.GetForEditAsync(1, "user1");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("T"));
        }

        [Test]
        public async Task GetForEditAsync_ReturnsNull_WhenNotOwner()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Name = "T", CreatorId = "owner" };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act
            var result = await service.GetForEditAsync(1, "user1");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task EditAsync_ValidData_UpdatesTournament()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, CreatorId = "user1", Name = "Old" };
            var model = new TournamentFormModel { Name = "New", FormatId = 1, LocationId = 1 };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act
            await service.EditAsync(1, model, "user1");

            // Assert
            Assert.That(tournament.Name, Is.EqualTo("New"));
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task EditAsync_Unauthorized_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, CreatorId = "owner", Name = "Old" };
            var model = new TournamentFormModel { Name = "New", FormatId = 1, LocationId = 1 };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.EditAsync(1, model, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Cannot edit this tournament."));
        }

        [Test]
        public async Task DeleteAsync_ValidData_MarksAsDeleted()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, CreatorId = "user1", IsDeleted = false };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act
            await service.DeleteAsync(1, "user1");

            // Assert
            Assert.That(tournament.IsDeleted, Is.True);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Unauthorized_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, CreatorId = "owner", IsDeleted = false };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(1, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Unauthorized."));
        }

        [Test]
        public async Task SetWinnerAsync_ValidData_SetsWinner()
        {
            // Arrange
            var player = new PlayerTournament { TournamentId = 1, PlayerId = "winner1" };
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Finished, CreatorId = "owner", PlayersTournaments = new List<PlayerTournament> { player } };
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(1)).ReturnsAsync(tournament);

            // Act
            await service.SetWinnerAsync(1, "winner1", "owner");

            // Assert
            Assert.That(tournament.WinnerId, Is.EqualTo("winner1"));
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task SetWinnerAsync_InvalidWinner_ThrowsException()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Finished, CreatorId = "owner", PlayersTournaments = new List<PlayerTournament>() };
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(1)).ReturnsAsync(tournament);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.SetWinnerAsync(1, "notAPlayer", "owner"));
            Assert.That(ex.Message, Is.EqualTo("Winner must be a player."));
        }

        [Test]
        public async Task DeleteUserRelatedDataAsync_CallsRepository()
        {
            // Arrange
            // No arrange setup required

            // Act
            await service.DeleteUserRelatedDataAsync("user1");

            // Assert
            mockRepo.Verify(r => r.RemoveUserRelatedDataAsync("user1"), Times.Once);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
