using Microsoft.EntityFrameworkCore;
using PokerTracker.Data.Models;
using PokerTracker.Data.Repository.Contracts;

namespace PokerTracker.Data.Repository
{
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<Location> GetAllLocationsQuery()
        {
            return dbContext.Locations.AsQueryable();
        }

        public async Task<Location?> GetByIdAsync(int id)
        {
            return await dbContext.Locations
                .Include(l => l.Tournaments)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task AddAsync(Location location)
        {
            await dbContext.Locations.AddAsync(location);
        }

        public async Task DeleteLocationAndTournamentsAsync(Location location)
        {
            // Soft delete the location
            location.IsDeleted = true;

            // Soft delete all associated tournaments
            if (location.Tournaments != null)
            {
                foreach (var tournament in location.Tournaments)
                {
                    tournament.IsDeleted = true;
                }
            }
            
            await Task.CompletedTask;
        }

        public new async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
