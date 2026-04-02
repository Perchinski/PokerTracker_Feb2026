using PokerTracker.ViewModels.Admin.Locations;

namespace PokerTracker.Services.Core.Contracts
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationViewModels>> GetAllLocationsAsync();
        Task CreateLocationAsync(LocationFormViewModel model);
        Task<LocationFormViewModel?> GetLocationForEditAsync(int id);
        Task UpdateLocationAsync(int id, LocationFormViewModel model);
        Task DeleteLocationAsync(int id);
    }
}
