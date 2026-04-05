using Moq;
using NUnit.Framework;
using PokerTracker.Data.Models;
using PokerTracker.Data.Repository.Contracts;
using PokerTracker.Services.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockQueryable;
using MockQueryable.Moq;

namespace PokerTracker.Services.Tests
{
    [TestFixture]
    public class LocationServiceTests
    {
        private Mock<ILocationRepository> mockRepo;
        private LocationService service;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<ILocationRepository>();
            service = new LocationService(mockRepo.Object);
        }

        [Test]
        public async Task GetAllLocationsAsync_ShouldReturnAllLocations()
        {
            // Arrange
            var locations = new List<Location>
            {
                new Location { Id = 1, Name = "Casino Royale", City = "Las Vegas", Address = "Strip 1", IsActive = true, Tournaments = new List<Tournament>() },
                new Location { Id = 2, Name = "Monte Carlo", City = "Monaco", Address = "City Center", IsActive = false, Tournaments = new List<Tournament>() }
            }.AsQueryable().BuildMock();

            mockRepo.Setup(r => r.GetAllLocationsQuery()).Returns(locations);

            // Act
            var result = await service.GetAllLocationsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Casino Royale"));
            Assert.That(result.Last().IsActive, Is.False);
        }

        [Test]
        public async Task GetLocationDetailsAsync_ShouldReturnDetails_WhenLocationExists()
        {
            // Arrange
            var location = new Location { Id = 1, Name = "Casino Royale", City = "Las Vegas", Address = "Strip 1", IsActive = true, Tournaments = new List<Tournament>() };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(location);

            // Act
            var result = await service.GetLocationDetailsAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Casino Royale"));
        }

        [Test]
        public async Task GetLocationDetailsAsync_ShouldReturnNull_WhenLocationDoesNotExist()
        {
            // Arrange
            mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Location)null);

            // Act
            var result = await service.GetLocationDetailsAsync(99);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateLocationAsync_ValidData_AddsLocation()
        {
            // Arrange
            var model = new PokerTracker.ViewModels.Admin.Locations.LocationFormViewModel
            {
                Name = "New Casino",
                Address = "New Address",
                City = "New City",
                ImageUrl = "img.jpg",
                IsActive = true
            };

            // Act
            await service.CreateLocationAsync(model);

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.Is<Location>(l => 
                l.Name == "New Casino" && 
                l.City == "New City" && 
                l.IsActive == true && 
                l.IsDeleted == false)), Times.Once);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task GetLocationForEditAsync_ReturnsModel_WhenLocationExists()
        {
            // Arrange
            var location = new Location { Id = 1, Name = "Casino", City = "City", Address = "Address", IsActive = true };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(location);

            // Act
            var result = await service.GetLocationForEditAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Casino"));
        }

        [Test]
        public async Task GetLocationForEditAsync_ReturnsNull_WhenLocationDoesNotExist()
        {
            // Arrange
            mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Location)null);

            // Act
            var result = await service.GetLocationForEditAsync(99);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateLocationAsync_ValidData_UpdatesLocation()
        {
            // Arrange
            var location = new Location { Id = 1, Name = "Old Name" };
            var model = new PokerTracker.ViewModels.Admin.Locations.LocationFormViewModel { Name = "New Name", City = "New City", Address = "New Address", IsActive = false };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(location);

            // Act
            await service.UpdateLocationAsync(1, model);

            // Assert
            Assert.That(location.Name, Is.EqualTo("New Name"));
            Assert.That(location.City, Is.EqualTo("New City"));
            Assert.That(location.IsActive, Is.False);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateLocationAsync_LocationNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var model = new PokerTracker.ViewModels.Admin.Locations.LocationFormViewModel { Name = "New" };
            mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Location)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<System.InvalidOperationException>(() => service.UpdateLocationAsync(99, model));
            Assert.That(ex.Message, Is.EqualTo("Location not found."));
        }

        [Test]
        public async Task DeleteLocationAsync_ValidId_DeletesLocation()
        {
            // Arrange
            var location = new Location { Id = 1, Name = "To Delete" };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(location);

            // Act
            await service.DeleteLocationAsync(1);

            // Assert
            mockRepo.Verify(r => r.DeleteLocationAndTournamentsAsync(location), Times.Once);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteLocationAsync_LocationNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Location)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<System.InvalidOperationException>(() => service.DeleteLocationAsync(99));
            Assert.That(ex.Message, Is.EqualTo("Location not found."));
        }

        [Test]
        public async Task GetLocationDetailsAsync_WithNullTournaments_ReturnsZero()
        {
            // Arrange
            var location = new Location
            {
                Id = 1,
                Name = "Casino Royale",
                City = "Las Vegas",
                Address = "Strip 1",
                IsActive = true,
                Tournaments = null
            };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(location);

            // Act
            var result = await service.GetLocationDetailsAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.TournamentCount, Is.EqualTo(0));
        }
    }
}
