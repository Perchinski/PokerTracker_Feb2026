using Microsoft.EntityFrameworkCore;
using PokerTracker.Data.Models;
using PokerTracker.Data.Repository.Contracts;
using PokerTracker.Services.Core.Contracts;
using PokerTracker.ViewModels.Announcements;

namespace PokerTracker.Services.Core;

public class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository repository;

    public AnnouncementService(IAnnouncementRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<AnnouncementViewModel>> GetAllAsync(bool includeInactive = false)
    {
        var query = repository.GetAllQuery();
        
        if (!includeInactive)
        {
            query = query.Where(a => a.IsActive);
        }

        return await query
            .OrderByDescending(a => a.PublishedOn)
            .Select(a => new AnnouncementViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                PublishedOn = a.PublishedOn,
                IsActive = a.IsActive
            })
            .ToListAsync();
    }

    public async Task<AnnouncementViewModel?> GetByIdAsync(int id)
    {
        var announcement = await repository.GetByIdAsync(id);
        
        if (announcement == null)
            return null;

        return new AnnouncementViewModel
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            PublishedOn = announcement.PublishedOn,
            IsActive = announcement.IsActive
        };
    }

    public async Task CreateAsync(AnnouncementViewModel model)
    {
        var announcement = new Announcement
        {
            Title = model.Title,
            Content = model.Content,
            PublishedOn = DateTime.UtcNow,
            IsActive = model.IsActive
        };

        await repository.AddAsync(announcement);
        await repository.SaveChangesAsync();
    }

    public async Task UpdateAsync(AnnouncementViewModel model)
    {
        var announcement = await repository.GetByIdAsync(model.Id);
        if (announcement != null)
        {
            announcement.Title = model.Title;
            announcement.Content = model.Content;
            announcement.IsActive = model.IsActive;

            await repository.UpdateAsync(announcement);
            await repository.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var announcement = await repository.GetByIdAsync(id);
        if (announcement != null)
        {
            await repository.DeleteAsync(announcement);
            await repository.SaveChangesAsync();
        }
    }
}
