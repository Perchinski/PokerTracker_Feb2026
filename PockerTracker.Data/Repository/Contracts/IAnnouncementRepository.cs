using PokerTracker.Data.Models;

namespace PokerTracker.Data.Repository.Contracts;

public interface IAnnouncementRepository
{
    IQueryable<Announcement> GetAllQuery();
    Task<Announcement?> GetByIdAsync(int id);
    Task AddAsync(Announcement announcement);
    Task UpdateAsync(Announcement announcement);
    Task DeleteAsync(Announcement announcement);
    Task SaveChangesAsync();
}
