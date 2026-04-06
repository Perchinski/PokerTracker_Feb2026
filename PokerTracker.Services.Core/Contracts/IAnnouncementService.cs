using PokerTracker.ViewModels.Announcements;

namespace PokerTracker.Services.Core.Contracts;

public interface IAnnouncementService
{
    Task<IEnumerable<AnnouncementViewModel>> GetAllAsync(bool includeInactive = false);
    Task<AnnouncementViewModel?> GetByIdAsync(int id);
    Task CreateAsync(AnnouncementViewModel model);
    Task UpdateAsync(AnnouncementViewModel model);
    Task DeleteAsync(int id);
}
