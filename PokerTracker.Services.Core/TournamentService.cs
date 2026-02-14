using Microsoft.EntityFrameworkCore;
using PokerTracker.Data;
using PokerTracker.Data.Models;
using PokerTracker.Services.Core.Contracts;
using PokerTracker.ViewModels.Tournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Services.Core
{
    public class TournamentService(ApplicationDbContext context) : ITournamentService
    {
        public async Task<IEnumerable<TournamentFormatViewModel>> GetFormatsAsync()
        {
            return await context.TournamentFormats
                .Select(f => new TournamentFormatViewModel
                {
                    Id = f.Id,
                    Name = f.Name
                })
                .ToListAsync();
        }

        public async Task CreateAsync(TournamentFormModel model, string userId)
        {
            var tournament = new Tournament
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Date = model.Date,
                FormatId = model.FormatId,
                CreatorId = userId,
                // WinnerId is null by default
                // IsDeleted is false by default
            };

            await context.Tournaments.AddAsync(tournament);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TournamentIndexViewModel>> GetAllTournamentsAsync()
        {
            return await context.Tournaments
        .Select(t => new TournamentIndexViewModel
        {
            Id = t.Id,
            Name = t.Name,
            Format = t.Format.Name,
            Creator = t.Creator.UserName!, 
            Date = t.Date,
            Status = t.Date < DateTime.Now ? "Finished" : "Open"
        })
        .OrderByDescending(t => t.Date)
        .ThenBy(t => t.Name)
        .AsNoTracking()
        .ToListAsync();
        }

    }
}
