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
                .AsNoTracking()
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
                CreatorId = userId
            };

            await context.Tournaments.AddAsync(tournament);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TournamentIndexViewModel>> GetAllTournamentsAsync()
        {
            var tournaments = await context.Tournaments
                .OrderByDescending(t => t.Date)
                .ThenBy(t => t.Name)
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                    FormatName = t.Format.Name,
                    CreatorName = t.Creator.UserName,
                    t.Date,
                    t.ImageUrl
                })
                .AsNoTracking()
                .ToListAsync();

            return tournaments.Select(t => new TournamentIndexViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Format = t.FormatName,
                Creator = t.CreatorName ?? "Unknown",
                Date = t.Date,
                ImageUrl = t.ImageUrl,
                Status = GetStatus(t.Date) 
            });
        }

        private static string GetStatus(DateTime startDate)
        {
            var now = DateTime.Now;

            if (startDate > now)
            {
                return "Open";
            }

            if (startDate < now && startDate.AddHours(3) > now)
            {
                return "Running";
            }

            return "Finished";
        }
    }
}
