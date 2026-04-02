using PokerTracker.Data.Models;

namespace PokerTracker.Data.Repository.Contracts
{
    public interface ILocationRepository
    {
        IQueryable<Location> GetAllLocationsQuery();
        Task<Location?> GetByIdAsync(int id);
        Task AddAsync(Location location);
        Task SaveChangesAsync();
        Task DeleteLocationAndTournamentsAsync(Location location);
    }
}
