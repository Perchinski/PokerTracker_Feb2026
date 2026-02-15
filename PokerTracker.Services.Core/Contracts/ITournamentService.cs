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

    }
}
