using PokerTracker.ViewModels.Admin.Locations;

namespace PokerTracker.Services.Core.Contracts
{
    /// <summary>
    /// Service contract for managing locations used in Poker Tournaments.
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Retrieves all available locations as view models.
        /// </summary>
        Task<IEnumerable<LocationViewModels>> GetAllLocationsAsync();
        /// <summary>
        /// Retrieves a location's detailed footprint by ID.
        /// </summary>
        /// <param name="id">The location's ID.</param>
        Task<LocationViewModels?> GetLocationDetailsAsync(int id);
        /// <summary>
        /// Persists a uniquely defined location from the form data.
        /// </summary>
        /// <param name="model">The location configuration to save.</param>
        Task CreateLocationAsync(LocationFormViewModel model);
        /// <summary>
        /// Fetches a specific location formed appropriately for editing its values.
        /// </summary>
        /// <param name="id">The location's ID.</param>
        Task<LocationFormViewModel?> GetLocationForEditAsync(int id);
        /// <summary>
        /// Applies the changes provided via form data to the specified location.
        /// </summary>
        /// <param name="id">The location's ID to be updated.</param>
        /// <param name="model">The updated configurations.</param>
        Task UpdateLocationAsync(int id, LocationFormViewModel model);
        /// <summary>
        /// Safely deletes a location and its strictly related data artifacts off the record.
        /// </summary>
        /// <param name="id">The location's ID.</param>
        Task DeleteLocationAsync(int id);
    }
}
