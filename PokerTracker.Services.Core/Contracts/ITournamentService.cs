using PokerTracker.ViewModels.Tournaments;
using PokerTracker.GCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Services.Core.Contracts
{
    /// <summary>
    /// Core tournament operations. All state-changing methods throw on
    /// authorization or business rule violations — callers must handle exceptions.
    /// </summary>
    public interface ITournamentService
    {
        /// <summary>
        /// Retrieves a list of all available tournament formats (e.g., Texas Hold'em, Omaha).
        /// </summary>
        /// <returns>A collection of tournament format view models.</returns>
        Task<IEnumerable<TournamentFormatViewModel>> GetFormatsAsync();

        /// <summary>
        /// Retrieves a list of all active physical or online locations available for hosting tournaments.
        /// </summary>
        /// <returns>A collection of location selection view models.</returns>
        Task<IEnumerable<LocationSelectionViewModel>> GetActiveLocationsAsync();

        /// <summary>
        /// Creates a new tournament record in the database.
        /// </summary>
        /// <param name="model">The tournament details submitted by the user.</param>
        /// <param name="userId">The ID of the user creating (and therefore owning) the tournament.</param>
        Task CreateAsync(TournamentFormModel model, string userId);

        /// <summary>
        /// Retrieves a paginated, filtered, and sorted list of tournaments.
        /// </summary>
        /// <returns>A tuple containing the list of tournaments and the total count of matched records.</returns>
        Task<(List<TournamentIndexViewModel> Tournaments, int TotalCount)> GetAllTournamentsAsync(string? searchTerm, int? formatId, string? status, string sortOrder, bool onlyJoined, bool onlyOwned, string? userId, bool isAdmin = false, int pageNumber = 1, int pageSize = ApplicationConstants.DefaultPageSize);

        /// <summary>
        /// Retrieves the full details of a specific tournament, including participants and location data.
        /// </summary>
        /// <param name="id">The tournament ID.</param>
        /// <param name="userId">The ID of the currently authenticated user (used to determine contextual permissions).</param>
        /// <param name="isAdmin">Indicates whether the current user is an administrator.</param>
        Task<TournamentDetailsViewModel?> GetDetailsAsync(int id, string? userId, bool isAdmin = false);

        /// <summary>
        /// Adds a user as a participant to a specific tournament.
        /// </summary>
        /// <param name="tournamentId">The tournament ID.</param>
        /// <param name="userId">The ID of the user joining the tournament.</param>
        Task JoinAsync(int tournamentId, string userId);

        /// <summary>
        /// Removes a user from the participant list of a specific tournament.
        /// </summary>
        /// <param name="tournamentId">The tournament ID.</param>
        /// <param name="userId">The ID of the user leaving the tournament.</param>
        Task LeaveAsync(int tournamentId, string userId);

        /// <summary>
        /// Allows a tournament owner or administrator to forcibly remove a participant from a tournament.
        /// </summary>
        /// <param name="tournamentId">The tournament ID.</param>
        /// <param name="playerIdToRemove">The user ID of the participant to remove.</param>
        /// <param name="currentUserId">The user ID of the person attempting the removal.</param>
        /// <param name="isAdmin">Indicates if the person attempting the removal is an admin.</param>
        Task RemovePlayerAsync(int tournamentId, string playerIdToRemove, string currentUserId, bool isAdmin = false);

        /// <summary>
        /// Retrieves a tournament's data mapped to a form model for editing purposes.
        /// </summary>
        /// <param name="id">The tournament ID.</param>
        /// <param name="userId">The ID of the user attempting to edit.</param>
        /// <param name="isAdmin">Indicates if the user is an admin.</param>
        Task<TournamentFormModel?> GetForEditAsync(int id, string userId, bool isAdmin = false);

        /// <summary>
        /// Updates an existing tournament's details.
        /// </summary>
        /// <param name="id">The tournament ID to update.</param>
        /// <param name="model">The new properties for the tournament.</param>
        /// <param name="userId">The user ID requesting the update.</param>
        /// <param name="isAdmin">Indicates if the user is an admin.</param>
        Task EditAsync(int id, TournamentFormModel model, string userId, bool isAdmin = false);

        /// <summary>
        /// Permanently deletes a tournament and its associated participant mappings.
        /// </summary>
        /// <param name="id">The tournament ID.</param>
        /// <param name="userId">The user ID requesting the deletion.</param>
        /// <param name="isAdmin">Indicates if the user is an admin.</param>
        Task DeleteAsync(int id, string userId, bool isAdmin = false);

        // Status transitions: Open → Running → Finished
        
        /// <summary>
        /// Transitions a tournament status to 'Running'.
        /// </summary>
        Task StartAsync(int id, string userId, bool isAdmin = false);
        
        /// <summary>
        /// Transitions a tournament status to 'Finished'.
        /// </summary>
        Task FinishAsync(int id, string userId, bool isAdmin = false);

        /// <summary>
        /// Sets a specific participant as the overall winner of a finished tournament.
        /// </summary>
        Task SetWinnerAsync(int tournamentId, string winnerId, string userId, bool isAdmin = false);

        /// <summary>
        /// Clears out all tournaments hosted by a user, and removes them from any tournaments they joined.
        /// Primarily used when a user's account is being deleted.
        /// </summary>
        Task DeleteUserRelatedDataAsync(string userId);
    }
}
