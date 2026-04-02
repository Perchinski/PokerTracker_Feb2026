using PokerTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Data.Repository.Contracts
{
    public interface ITournamentRepository
    {
        // --- Tournament Queries ---
        IQueryable<Tournament> GetAllTournamentsQuery();
        Task<Tournament?> GetByIdAsync(int id);
        Task<Tournament?> GetByIdWithPlayersAsync(int id);
        Task<Tournament?> GetDetailsByIdAsync(int id);

        // --- Format Queries ---
        IQueryable<TournamentFormat> GetAllFormatsQuery();
        Task<bool> FormatExistsAsync(int formatId);

        IQueryable<Location> GetActiveLocationsQuery();
        Task<bool> LocationExistsAsync(int locationId);

        // --- Write Operations ---
        Task AddAsync(Tournament tournament);
        Task SaveChangesAsync();
        Task RemoveUserRelatedDataAsync(string userId);
    }
}
