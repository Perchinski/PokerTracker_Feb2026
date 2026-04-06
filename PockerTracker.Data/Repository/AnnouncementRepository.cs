using Microsoft.EntityFrameworkCore;
using PokerTracker.Data.Models;
using PokerTracker.Data.Repository.Contracts;

namespace PokerTracker.Data.Repository;

public class AnnouncementRepository : BaseRepository, IAnnouncementRepository
{
    public AnnouncementRepository(ApplicationDbContext context) : base(context)
    {
    }

    public IQueryable<Announcement> GetAllQuery()
    {
        return dbContext.Announcements;
    }

    public async Task<Announcement?> GetByIdAsync(int id)
    {
        return await dbContext.Announcements.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Announcement announcement)
    {
        await dbContext.Announcements.AddAsync(announcement);
    }

    public Task UpdateAsync(Announcement announcement)
    {
        dbContext.Announcements.Update(announcement);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Announcement announcement)
    {
        announcement.IsDeleted = true;
        dbContext.Announcements.Update(announcement);
        return Task.CompletedTask;
    }

    public new async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}
