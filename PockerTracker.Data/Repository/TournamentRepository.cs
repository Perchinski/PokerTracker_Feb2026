using Microsoft.EntityFrameworkCore;
using PokerTracker.Data.Models;
using PokerTracker.Data.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Data.Repository
{
    public class TournamentRepository : BaseRepository, ITournamentRepository
    {
        public TournamentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<Tournament> GetAllTournamentsQuery()
        {
            return dbContext!.Tournaments
                .Include(t => t.Format)
                .Include(t => t.PlayersTournaments)
                .AsQueryable();
        }

        public async Task<Tournament?> GetByIdAsync(int id)
        {
            return await dbContext!.Tournaments.FindAsync(id);
        }

        public async Task<Tournament?> GetByIdWithPlayersAsync(int id)
        {
            return await dbContext!.Tournaments
                .Include(t => t.PlayersTournaments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tournament?> GetDetailsByIdAsync(int id)
        {
            return await dbContext!.Tournaments
                .Include(t => t.Format)
                .Include(t => t.Creator)
                .Include(t => t.Winner)
                .Include(t => t.PlayersTournaments)
                    .ThenInclude(pt => pt.Player)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public IQueryable<TournamentFormat> GetAllFormatsQuery()
        {
            return dbContext!.TournamentFormats.AsQueryable();
        }

        public async Task<bool> FormatExistsAsync(int formatId)
        {
            return await dbContext!.TournamentFormats.AnyAsync(f => f.Id == formatId);
        }

        public async Task AddAsync(Tournament tournament)
        {
            await dbContext!.Tournaments.AddAsync(tournament);
        }

        public new async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}

