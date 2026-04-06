using Moq;
using NUnit.Framework;
using PokerTracker.Data.Models;
using PokerTracker.Data.Repository.Contracts;
using PokerTracker.Services.Core;
using PokerTracker.ViewModels.Announcements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockQueryable;
using MockQueryable.Moq;

namespace PokerTracker.Services.Tests
{
    [TestFixture]
    public class AnnouncementServiceTests
    {
        private Mock<IAnnouncementRepository> mockRepo;
        private AnnouncementService service;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IAnnouncementRepository>();
            service = new AnnouncementService(mockRepo.Object);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnActiveAnnouncements_WhenIncludeInactiveIsFalse()
        {
            // Arrange
            var announcements = new List<Announcement>
            {
                new Announcement { Id = 1, Title = "Active 1", Content = "Content 1", IsActive = true, PublishedOn = DateTime.UtcNow },
                new Announcement { Id = 2, Title = "Inactive 1", Content = "Content 2", IsActive = false, PublishedOn = DateTime.UtcNow },
                new Announcement { Id = 3, Title = "Active 2", Content = "Content 3", IsActive = true, PublishedOn = DateTime.UtcNow }
            }.AsQueryable().BuildMock();

            mockRepo.Setup(r => r.GetAllQuery()).Returns(announcements);

            // Act
            var result = await service.GetAllAsync(includeInactive: false);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.All(a => a.IsActive), Is.True);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllAnnouncements_WhenIncludeInactiveIsTrue()
        {
            // Arrange
            var announcements = new List<Announcement>
            {
                new Announcement { Id = 1, Title = "Active 1", Content = "Content 1", IsActive = true, PublishedOn = DateTime.UtcNow },
                new Announcement { Id = 2, Title = "Inactive 1", Content = "Content 2", IsActive = false, PublishedOn = DateTime.UtcNow },
                new Announcement { Id = 3, Title = "Active 2", Content = "Content 3", IsActive = true, PublishedOn = DateTime.UtcNow }
            }.AsQueryable().BuildMock();

            mockRepo.Setup(r => r.GetAllQuery()).Returns(announcements);

            // Act
            var result = await service.GetAllAsync(includeInactive: true);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnViewModel_WhenAnnouncementExists()
        {
            // Arrange
            var id = 1;
            var announcement = new Announcement { Id = id, Title = "Test", Content = "Test Content", IsActive = true, PublishedOn = DateTime.UtcNow };

            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(announcement);

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(id));
            Assert.That(result.Title, Is.EqualTo("Test"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenAnnouncementDoesNotExist()
        {
            // Arrange
            var id = 99;

            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Announcement?)null);

            // Act
            var result = await service.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateAsync_ShouldAddAnnouncementAndSaveChanges()
        {
            // Arrange
            var viewModel = new AnnouncementViewModel
            {
                Title = "New Announcement",
                Content = "New Content",
                IsActive = true
            };

            // Act
            await service.CreateAsync(viewModel);

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.Is<Announcement>(a => 
                a.Title == "New Announcement" && 
                a.Content == "New Content" && 
                a.IsActive == true)), Times.Once);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateAnnouncementAndSaveChanges_WhenExists()
        {
            // Arrange
            var id = 1;
            var existingAnnouncement = new Announcement { Id = id, Title = "Old Title", Content = "Old Content", IsActive = false };
            
            var viewModel = new AnnouncementViewModel
            {
                Id = id,
                Title = "Updated Title",
                Content = "Updated Content",
                IsActive = true
            };

            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingAnnouncement);

            // Act
            await service.UpdateAsync(viewModel);

            // Assert
            Assert.That(existingAnnouncement.Title, Is.EqualTo("Updated Title"));
            Assert.That(existingAnnouncement.Content, Is.EqualTo("Updated Content"));
            Assert.That(existingAnnouncement.IsActive, Is.True);
            
            mockRepo.Verify(r => r.UpdateAsync(existingAnnouncement), Times.Once);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ShouldNotUpdate_WhenDoesNotExist()
        {
            // Arrange
            var id = 99;
            var viewModel = new AnnouncementViewModel { Id = id };

            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Announcement?)null);

            // Act
            await service.UpdateAsync(viewModel);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Announcement>()), Times.Never);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task DeleteAsync_ShouldCallDeleteAndSaveChanges_WhenExists()
        {
            // Arrange
            var id = 1;
            var existingAnnouncement = new Announcement { Id = id, Title = "To Delete" };

            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingAnnouncement);

            // Act
            await service.DeleteAsync(id);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(existingAnnouncement), Times.Once);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ShouldNotDelete_WhenDoesNotExist()
        {
            // Arrange
            var id = 99;

            mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Announcement?)null);

            // Act
            await service.DeleteAsync(id);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Announcement>()), Times.Never);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}