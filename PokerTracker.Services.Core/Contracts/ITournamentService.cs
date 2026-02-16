using PokerTracker.ViewModels.Tournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Services.Core.Contracts
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentFormatViewModel>> GetFormatsAsync();

        Task CreateAsync(TournamentFormModel model, string userId);

        Task<IEnumerable<TournamentIndexViewModel>> GetAllTournamentsAsync();

        Task<TournamentDetailsViewModel?> GetDetailsAsync(int id, string? userId);

        Task JoinAsync(int tournamentId, string userId);

        Task LeaveAsync(int tournamentId, string userId);

        Task<TournamentFormModel?> GetForEditAsync(int id, string userId);

        Task EditAsync(int id, TournamentFormModel model, string userId);

        Task DeleteAsync(int id, string userId);

        Task StartAsync(int id, string userId);
        Task FinishAsync(int id, string userId);

    }
}
