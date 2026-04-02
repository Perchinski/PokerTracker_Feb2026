using Microsoft.EntityFrameworkCore;
using PokerTracker.Data.Models;
using PokerTracker.Data.Repository.Contracts;
using PokerTracker.Services.Core.Contracts;
using PokerTracker.ViewModels.Admin.Locations;

namespace PokerTracker.Services.Core
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository repository;

        public LocationService(ILocationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<LocationViewModels>> GetAllLocationsAsync()
        {
            return await repository.GetAllLocationsQuery()
                .Select(l => new LocationViewModels
                {
                    Id = l.Id,
                    Name = l.Name,
                    City = l.City,
                    Address = l.Address,
                    ImageUrl = l.ImageUrl,
                    IsActive = l.IsActive,
                    TournamentCount = l.Tournaments.Count
                })
                .ToListAsync();
        }

        public async Task<LocationViewModels?> GetLocationDetailsAsync(int id)
        {
            var location = await repository.GetByIdAsync(id);
            if (location == null) return null;

            return new LocationViewModels
            {
                Id = location.Id,
                Name = location.Name,
                City = location.City,
                Address = location.Address,
                ImageUrl = location.ImageUrl,
                IsActive = location.IsActive,
                TournamentCount = location.Tournaments?.Count ?? 0
            };
        }

        public async Task CreateLocationAsync(LocationFormViewModel model)
        {
            var location = new Location
            {
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                ImageUrl = model.ImageUrl,
                IsActive = model.IsActive,
                IsDeleted = false
            };

            await repository.AddAsync(location);
            await repository.SaveChangesAsync();
        }

        public async Task<LocationFormViewModel?> GetLocationForEditAsync(int id)
        {
            var location = await repository.GetByIdAsync(id);
            if (location == null) return null;

            return new LocationFormViewModel
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
                City = location.City,
                ImageUrl = location.ImageUrl,
                IsActive = location.IsActive
            };
        }

        public async Task UpdateLocationAsync(int id, LocationFormViewModel model)
        {
            var location = await repository.GetByIdAsync(id);
            if (location == null) throw new InvalidOperationException("Location not found.");

            location.Name = model.Name;
            location.Address = model.Address;
            location.City = model.City;
            location.ImageUrl = model.ImageUrl;
            location.IsActive = model.IsActive;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteLocationAsync(int id)
        {
            var location = await repository.GetByIdAsync(id);
            if (location == null) throw new InvalidOperationException("Location not found.");

            await repository.DeleteLocationAndTournamentsAsync(location);
            await repository.SaveChangesAsync();
        }
    }
}
