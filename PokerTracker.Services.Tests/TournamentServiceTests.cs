using Microsoft.EntityFrameworkCore;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using PokerTracker.Data.Models;
using PokerTracker.Data.Repository.Contracts;
using PokerTracker.GCommon;
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

        [Test]
        public async Task GetAllTournamentsAsync_WithFiltersAndAdmin_ReturnsCorrectData()
        {
            // Arrange
            var testUser = new Microsoft.AspNetCore.Identity.IdentityUser { Id = "user1", UserName = "tester" };
            var format = new TournamentFormat { Id = 1, Name = "Format" };
            var location = new Location { Id = 1, Name = "Loc" };
            var pt = new PlayerTournament { PlayerId = "user1", Player = testUser, TournamentId = 1 };

            var query = new List<Tournament>
            {
                new Tournament
                {
                    Id = 1, Name = "Big Poker Night", Description = "Huge event",
                    Date = DateTime.Now, FormatId = 1, Format = format,
                    LocationId = 1, Location = location, Status = TournamentStatus.Open,
                    CreatorId = "user1", Creator = testUser,
                    PlayersTournaments = new List<PlayerTournament> { pt }
                },
                new Tournament
                {
                    Id = 2, Name = "Small Game", Description = "Tiny",
                    Date = DateTime.Now.AddDays(1), FormatId = 2, Format = new TournamentFormat { Id=2, Name="Other"},
                    LocationId = 1, Location = location, Status = TournamentStatus.Finished,
                    CreatorId = "otherUser", Creator = new Microsoft.AspNetCore.Identity.IdentityUser(),
                    PlayersTournaments = new List<PlayerTournament>()
                }
            }.AsQueryable().BuildMock();

            mockRepo.Setup(r => r.GetAllTournamentsQuery()).Returns(query);

            // Act
            var (tournaments, totalCount) = await service.GetAllTournamentsAsync(
                "Big", 1, "Open", "DateDescending", true, true, "user1", isAdmin: true, 1, 10);

            // Assert
            Assert.That(totalCount, Is.EqualTo(1));
            Assert.That(tournaments.First().Name, Is.EqualTo("Big Poker Night"));
            Assert.That(tournaments.First().IsOwner, Is.True);
        }

        [Test]
        public void JoinAsync_AlreadyJoined_ThrowsInvalidOperationException()
        {
            // Arrange
            var pt = new PlayerTournament { PlayerId = "user1", TournamentId = 1 };
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Open, PlayersTournaments = new List<PlayerTournament> { pt } };
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(1)).ReturnsAsync(tournament);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.JoinAsync(1, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Already joined."));
        }

        [Test]
        public void LeaveAsync_NotRegistered_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Open, PlayersTournaments = new List<PlayerTournament>() };
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(1)).ReturnsAsync(tournament);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.LeaveAsync(1, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Not registered."));
        }

        [Test]
        public void RemovePlayerAsync_NotAdminOrOwner_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Open, CreatorId = "ownerId" };
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(1)).ReturnsAsync(tournament);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.RemovePlayerAsync(1, "somePlayer", "intruder", isAdmin: false));
            Assert.That(ex.Message, Is.EqualTo("Only the owner or an admin can remove players."));
        }

        [Test]
        public async Task EditAsync_AsAdmin_AllowsEditingOthersTournaments()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, CreatorId = "otherUser", Name = "Old" };
            var model = new TournamentFormModel { Name = "New", FormatId = 1, LocationId = 1 };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);
            // mockRepo setup for Format/Location exists is already in [SetUp]

            // Act - user1 is NOT the creator, but isAdmin is TRUE
            await service.EditAsync(1, model, "user1", isAdmin: true);

            // Assert
            Assert.That(tournament.Name, Is.EqualTo("New"));
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void EditAsync_InvalidFormat_ThrowsArgumentException()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, CreatorId = "user1" };
            var model = new TournamentFormModel { FormatId = 99, LocationId = 1 };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);
            mockRepo.Setup(r => r.FormatExistsAsync(99)).ReturnsAsync(false); // Force format failure

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => service.EditAsync(1, model, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Invalid format."));
        }

        [Test]
        public async Task GetDetailsAsync_WithFullData_CoversAllBranchesAndLambdas()
        {
            // Arrange
            var format = new TournamentFormat { Id = 1, Name = "Format" };
            var testUser = new Microsoft.AspNetCore.Identity.IdentityUser { Id = "owner", UserName = "tester" };
            var playerUser = new Microsoft.AspNetCore.Identity.IdentityUser { Id = "player1", UserName = "playerOne" };
            var location = new Location { Id = 1, Name = "Vegas", Address = "123 Strip", City = "LV", ImageUrl = "vegas.jpg" };

            var tournament = new Tournament
            {
                Id = 1,
                Name = "Big One",
                Description = null,
                Date = DateTime.Now,
                FormatId = 1,
                Format = format,
                LocationId = 1,
                Location = location,
                Status = TournamentStatus.Open,
                CreatorId = "owner",
                Creator = testUser,
                WinnerId = "player1",
                Winner = playerUser,
                PlayersTournaments = new List<PlayerTournament>
                {
                    new PlayerTournament { PlayerId = "player1", Player = playerUser }
                }
            };

            mockRepo.Setup(r => r.GetDetailsByIdAsync(1)).ReturnsAsync(tournament);

            // Act
            var result = await service.GetDetailsAsync(1, "user1");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Description, Is.EqualTo("No description provided."));
            Assert.That(result.LocationName, Is.EqualTo("Vegas"));
            Assert.That(result.WinnerName, Is.EqualTo("playerOne"));
            Assert.That(result.Players.Count, Is.EqualTo(1));
            Assert.That(result.Players.First().Name, Is.EqualTo("playerOne"));
        }

        [Test]
        public void JoinAsync_TournamentNotFound_ThrowsArgumentException()
        {
            // Arrange
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(99)).ReturnsAsync((Tournament)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => service.JoinAsync(99, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Tournament not found"));
        }

        [Test]
        public void LeaveAsync_TournamentNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(99)).ReturnsAsync((Tournament)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.LeaveAsync(99, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Cannot leave this tournament."));
        }

        [Test]
        public void EditAsync_TournamentNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Tournament)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.EditAsync(99, new TournamentFormModel(), "user1"));
            Assert.That(ex.Message, Is.EqualTo("Cannot edit this tournament."));
        }

        [Test]
        public void RemovePlayerAsync_TournamentFinished_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Finished, CreatorId = "owner" };
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(1)).ReturnsAsync(tournament);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.RemovePlayerAsync(1, "player", "owner"));
            Assert.That(ex.Message, Is.EqualTo("Players can only be removed while the tournament is open or running."));
        }

        [Test]
        public void RemovePlayerAsync_PlayerNotRegistered_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Open, CreatorId = "owner", PlayersTournaments = new List<PlayerTournament>() };
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(1)).ReturnsAsync(tournament);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.RemovePlayerAsync(1, "ghost", "owner"));
            Assert.That(ex.Message, Is.EqualTo("Player is not registered."));
        }

        [Test]
        public void EditAsync_InvalidLocation_ThrowsArgumentException()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, CreatorId = "user1" };
            var model = new TournamentFormModel { FormatId = 1, LocationId = 99 };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);
            mockRepo.Setup(r => r.FormatExistsAsync(1)).ReturnsAsync(true);
            mockRepo.Setup(r => r.LocationExistsAsync(99)).ReturnsAsync(false);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => service.EditAsync(1, model, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Invalid location. (It might be closed or archived)."));
        }

        [Test]
        public async Task GetAllTournamentsAsync_Sorting_CoversRemainingSortOrders()
        {
            // Arrange
            var format = new TournamentFormat { Id = 1, Name = "Format" };
            var location = new Location { Id = 1, Name = "Loc" };
            var testUser = new Microsoft.AspNetCore.Identity.IdentityUser { Id = "u1", UserName = "tester" };

            var query = new List<Tournament>
            {
                new Tournament { Id = 1, Name = "A", Date = DateTime.Now, Status = TournamentStatus.Open, Format = format, Location = location, Creator = testUser, PlayersTournaments = new List<PlayerTournament>() }
            }.AsQueryable().BuildMock();

            mockRepo.Setup(r => r.GetAllTournamentsQuery()).Returns(query);

            // Act
            await service.GetAllTournamentsAsync(null, null, null, "NameAscending", false, false, "u1");
            await service.GetAllTournamentsAsync(null, null, null, "SomethingRandom", false, false, "u1");

            // Assert
            mockRepo.Verify(r => r.GetAllTournamentsQuery(), Times.Exactly(2));
        }

        [Test]
        public void StartAsync_TournamentNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Tournament)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.StartAsync(99, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Only the creator or an admin can start an open tournament."));
        }

        [Test]
        public void StartAsync_InvalidStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Finished, CreatorId = "user1" };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.StartAsync(1, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Only the creator or an admin can start an open tournament."));
        }

        [Test]
        public async Task GetAllTournamentsAsync_SortDateAscending_CoversSwitchCase()
        {
            // Arrange
            var query = new List<Tournament>().AsQueryable().BuildMock();
            mockRepo.Setup(r => r.GetAllTournamentsQuery()).Returns(query);

            // Act
            await service.GetAllTournamentsAsync(null, null, null, ApplicationConstants.SortOrders.DateAscending, false, false, "u1");

            // Assert
            mockRepo.Verify(r => r.GetAllTournamentsQuery(), Times.Once);
        }

        [Test]
        public async Task GetAllTournamentsAsync_SortDateDescending_CoversSwitchCase()
        {
            // Arrange
            var query = new List<Tournament>().AsQueryable().BuildMock();
            mockRepo.Setup(r => r.GetAllTournamentsQuery()).Returns(query);

            // Act
            await service.GetAllTournamentsAsync(null, null, null, ApplicationConstants.SortOrders.DateDescending, false, false, "u1");

            // Assert
            mockRepo.Verify(r => r.GetAllTournamentsQuery(), Times.Once);
        }

        [Test]
        public async Task GetAllTournamentsAsync_SortNameAscending_CoversSwitchCase()
        {
            // Arrange
            var query = new List<Tournament>().AsQueryable().BuildMock();
            mockRepo.Setup(r => r.GetAllTournamentsQuery()).Returns(query);

            // Act
            await service.GetAllTournamentsAsync(null, null, null, ApplicationConstants.SortOrders.NameAscending, false, false, "u1");

            // Assert
            mockRepo.Verify(r => r.GetAllTournamentsQuery(), Times.Once);
        }

        [Test]
        public async Task GetAllTournamentsAsync_SortDefault_CoversSwitchCase()
        {
            // Arrange
            var query = new List<Tournament>().AsQueryable().BuildMock();
            mockRepo.Setup(r => r.GetAllTournamentsQuery()).Returns(query);

            // Act
            await service.GetAllTournamentsAsync(null, null, null, "SomeRandomSortOrder", false, false, "u1");

            // Assert
            mockRepo.Verify(r => r.GetAllTournamentsQuery(), Times.Once);
        }

        [Test]
        public void RemovePlayerAsync_TournamentNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(99)).ReturnsAsync((Tournament)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.RemovePlayerAsync(99, "player1", "user1"));
            Assert.That(ex.Message, Is.EqualTo("Tournament not found."));
        }

        [Test]
        public async Task GetDetailsAsync_WithNullProperties_CoversAlternativeBranches()
        {
            // Arrange
            var format = new TournamentFormat { Id = 1, Name = "Format" };

            // Users with explicitly null UserNames
            var creatorWithNullName = new Microsoft.AspNetCore.Identity.IdentityUser { Id = "creatorId", UserName = null };
            var playerWithNullName = new Microsoft.AspNetCore.Identity.IdentityUser { Id = "playerId", UserName = null };

            var tournament = new Tournament
            {
                Id = 1,
                Name = "Test",
                Description = "A valid description",
                Date = DateTime.Now,
                FormatId = 1,
                Format = format,
                LocationId = 1,
                Location = null,
                Status = TournamentStatus.Open,
                CreatorId = "creatorId",
                Creator = creatorWithNullName,
                WinnerId = null,
                Winner = null,
                PlayersTournaments = new List<PlayerTournament>
                {
                    new PlayerTournament { PlayerId = "playerId", Player = playerWithNullName }
                }
            };

            mockRepo.Setup(r => r.GetDetailsByIdAsync(1)).ReturnsAsync(tournament);

            // Act
            var result = await service.GetDetailsAsync(1, null, isAdmin: true);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Description, Is.EqualTo("A valid description"));
            Assert.That(result.LocationName, Is.EqualTo("TBD"));
            Assert.That(result.LocationAddress, Is.EqualTo(string.Empty));
            Assert.That(result.LocationCity, Is.EqualTo(string.Empty));
            Assert.That(result.Creator, Is.EqualTo("Unknown"));
            Assert.That(result.WinnerName, Is.Null);
            Assert.That(result.Players.First().Name, Is.EqualTo("Unknown Player"));
            Assert.That(result.IsJoined, Is.False);
            Assert.That(result.IsOwner, Is.True);
        }

        [Test]
        public async Task RemovePlayerAsync_AsAdmin_BypassesCreatorCheck()
        {
            // Arrange
            var player = new PlayerTournament { TournamentId = 1, PlayerId = "userToRemove" };
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Open, CreatorId = "owner", PlayersTournaments = new List<PlayerTournament> { player } };
            mockRepo.Setup(r => r.GetByIdWithPlayersAsync(1)).ReturnsAsync(tournament);

            // Act
            await service.RemovePlayerAsync(1, "userToRemove", "adminUser", isAdmin: true);

            // Assert
            Assert.That(tournament.PlayersTournaments.Count, Is.EqualTo(0));
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task GetForEditAsync_TournamentNotFound_ReturnsNull()
        {
            // Arrange
            mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Tournament)null);

            // Act
            var result = await service.GetForEditAsync(99, "user1");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetForEditAsync_AsAdmin_ReturnsModel_WhenNotOwner()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Name = "T", CreatorId = "owner" };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act
            var result = await service.GetForEditAsync(1, "adminUser", isAdmin: true);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("T"));
        }

        [Test]
        public void DeleteAsync_TournamentNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Tournament)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(99, "user1"));
            Assert.That(ex.Message, Is.EqualTo("Unauthorized."));
        }

        [Test]
        public async Task DeleteAsync_AsAdmin_MarksAsDeleted_WhenNotOwner()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, CreatorId = "owner", IsDeleted = false };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act
            await service.DeleteAsync(1, "adminUser", isAdmin: true);

            // Assert
            Assert.That(tournament.IsDeleted, Is.True);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task StartAsync_AsAdmin_BypassesCreatorCheck()
        {
            // Arrange
            var tournament = new Tournament { Id = 1, Status = TournamentStatus.Open, CreatorId = "owner" };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tournament);

            // Act
            await service.StartAsync(1, "adminUser", isAdmin: true);

            // Assert
            Assert.That(tournament.Status, Is.EqualTo(TournamentStatus.Running));
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
