using PokerTracker.ViewModels.Tournaments;
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
        Task<IEnumerable<TournamentFormatViewModel>> GetFormatsAsync();

        Task CreateAsync(TournamentFormModel model, string userId);

        Task<List<TournamentIndexViewModel>> GetAllTournamentsAsync(string? searchTerm, int? formatId, string? status, string sortOrder, bool onlyJoined, bool onlyOwned, string? userId, bool isAdmin = false);

        Task<TournamentDetailsViewModel?> GetDetailsAsync(int id, string? userId, bool isAdmin = false);

        Task JoinAsync(int tournamentId, string userId);

        Task LeaveAsync(int tournamentId, string userId);

        Task RemovePlayerAsync(int tournamentId, string playerIdToRemove, string currentUserId, bool isAdmin = false);

        Task<TournamentFormModel?> GetForEditAsync(int id, string userId, bool isAdmin = false);

        Task EditAsync(int id, TournamentFormModel model, string userId, bool isAdmin = false);

        Task DeleteAsync(int id, string userId, bool isAdmin = false);

        // Status transitions: Open → Running → Finished
        Task StartAsync(int id, string userId, bool isAdmin = false);
        Task FinishAsync(int id, string userId, bool isAdmin = false);

        Task SetWinnerAsync(int tournamentId, string winnerId, string userId, bool isAdmin = false);

        Task DeleteUserRelatedDataAsync(string userId);
    }
}
