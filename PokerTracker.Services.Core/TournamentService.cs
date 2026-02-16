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

        public async Task<List<TournamentIndexViewModel>> GetAllTournamentsAsync(string? searchTerm, int? formatId, string? status, string sortOrder, bool onlyJoined, string? userId)
        {
            var query = context.Tournaments
                .Include(t => t.Format)
                .Include(t => t.PlayersTournaments)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearch = searchTerm.Trim().ToLower();

                query = query.Where(t => t.Name.ToLower().Contains(normalizedSearch)
                                      || t.Description.ToLower().Contains(normalizedSearch));
            }
            if (onlyJoined && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(t => t.PlayersTournaments.Any(pt => pt.PlayerId == userId));
            }
            if (formatId.HasValue)
            {
                query = query.Where(t => t.FormatId == formatId.Value);
            }

            if (!string.IsNullOrEmpty(status) && Enum.TryParse(typeof(TournamentStatus), status, out var statusEnum))
            {
                var validStatus = (TournamentStatus)statusEnum;
                query = query.Where(t => t.Status == validStatus);
            }

            query = sortOrder switch
            {
                "date_asc" => query.OrderBy(t => t.Date),
                "date_desc" => query.OrderByDescending(t => t.Date),
                "name_asc" => query.OrderBy(t => t.Name),
                "status" => query.OrderBy(t => t.Status).ThenBy(t => t.Date), 
                _ => query.OrderBy(t => t.Status).ThenBy(t => t.Date)
            };

            return await query
                .Select(t => new TournamentIndexViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Date = t.Date,
                    Format = t.Format.Name,
                    Status = t.Status.ToString(),
                    PlayersCount = t.PlayersTournaments.Count,
                    ImageUrl = t.ImageUrl,
                    Creator = t.Creator.UserName ?? "Unknown",
                    IsJoined = userId != null && t.PlayersTournaments.Any(pt => pt.PlayerId == userId),
                    IsOwner = t.CreatorId == userId,
                })
                .ToListAsync();
        }
        public async Task StartAsync(int id, string userId)
        {
            var tournament = await context.Tournaments.FindAsync(id);

            // Only Owner can start, and only if currently Open
            if (tournament == null || tournament.CreatorId != userId || tournament.Status != TournamentStatus.Open)
            {
                return;
            }

            tournament.Status = TournamentStatus.Running;
            await context.SaveChangesAsync();
        }

        public async Task FinishAsync(int id, string userId)
        {
            var tournament = await context.Tournaments.FindAsync(id);

            // Only Owner can finish, and only if currently Running
            if (tournament == null || tournament.CreatorId != userId || tournament.Status != TournamentStatus.Running)
            {
                return;
            }

            tournament.Status = TournamentStatus.Finished;
            await context.SaveChangesAsync();
        }

        //private static string GetStatus(DateTime startDate)
        //{
        //    var now = DateTime.Now;

        //    if (startDate > now)
        //    {
        //        return "Open";
        //    }

        //    if (startDate < now && startDate.AddHours(3) > now)
        //    {
        //        return "Running";
        //    }

        //    return "Finished";
        //}

        public async Task<TournamentDetailsViewModel?> GetDetailsAsync(int id, string? userId)
        {
            var tournament = await context.Tournaments
                .Include(t => t.Format)
                .Include(t => t.Creator)
                .Include(t => t.Winner)
                .Include(t => t.PlayersTournaments)
                    .ThenInclude(pt => pt.Player) // Load the actual Users
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tournament == null)
            {
                return null;
            }

            return new TournamentDetailsViewModel
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description ?? "No description provided.",
                Format = tournament.Format.Name,
                Creator = tournament.Creator.UserName ?? "Unknown",
                Date = tournament.Date,
                ImageUrl = tournament.ImageUrl,
                Status = tournament.Status.ToString(),
                IsJoined = userId != null && tournament.PlayersTournaments.Any(pt => pt.PlayerId == userId),
                IsOwner = tournament.CreatorId == userId,
                WinnerName = tournament.Winner?.UserName,
                Players = tournament.PlayersTournaments.Select(pt => new PlayerViewModel
                {
                    Id = pt.PlayerId,
                    Name = pt.Player.UserName ?? "Unknown Player"
                }).ToList()
            };
        }

        public async Task JoinAsync(int tournamentId, string userId)
        {
            var tournament = await context.Tournaments
                .Include(t => t.PlayersTournaments)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament == null)
            {
                throw new ArgumentException("Tournament not found");
            }

            if (tournament.PlayersTournaments.Any(pt => pt.PlayerId == userId))
            {
                return;
            }

            tournament.PlayersTournaments.Add(new PlayerTournament
            {
                TournamentId = tournamentId,
                PlayerId = userId
            });

            await context.SaveChangesAsync();
        }

        public async Task LeaveAsync(int tournamentId, string userId)
        {
            var tournament = await context.Tournaments
                .Include(t => t.PlayersTournaments)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament == null)
            {
                return;
            }

            var playerTournament = tournament.PlayersTournaments
                .FirstOrDefault(pt => pt.PlayerId == userId);

            if (playerTournament != null)
            {
                tournament.PlayersTournaments.Remove(playerTournament);
                await context.SaveChangesAsync();
            }
        }
        public async Task<TournamentFormModel?> GetForEditAsync(int id, string userId)
        {
            var tournament = await context.Tournaments.FindAsync(id);

            if (tournament == null || tournament.CreatorId != userId)
            {
                return null;
            }

            return new TournamentFormModel
            {
                Name = tournament.Name,
                Description = tournament.Description,
                Date = tournament.Date,
                FormatId = tournament.FormatId,
                ImageUrl = tournament.ImageUrl
            };
        }

        public async Task EditAsync(int id, TournamentFormModel model, string userId)
        {
            var tournament = await context.Tournaments.FindAsync(id);

            if (tournament == null || tournament.CreatorId != userId)
            {
                throw new InvalidOperationException("Unauthorized or Tournament not found.");
            }

            if (tournament.Status != TournamentStatus.Open)
            {
                throw new InvalidOperationException("Cannot edit a tournament that has already started or finished.");
            }

            tournament.Name = model.Name;
            tournament.Description = model.Description;
            tournament.Date = model.Date;
            tournament.FormatId = model.FormatId;
            tournament.ImageUrl = model.ImageUrl;

            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id, string userId)
        {
            var tournament = await context.Tournaments.FindAsync(id);

            if (tournament == null || tournament.CreatorId != userId || userId == null)
            {
                throw new InvalidOperationException("Unauthorized or Tournament not found.");
            }

            tournament.IsDeleted = true;

            await context.SaveChangesAsync();
        }
        public async Task SetWinnerAsync(int tournamentId, string winnerId, string userId)
        {
            var tournament = await context.Tournaments
                .Include(t => t.PlayersTournaments)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            // Security check: Must exist, must be owner, must be finished
            if (tournament == null || tournament.CreatorId != userId || tournament.Status != TournamentStatus.Finished)
            {
                throw new InvalidOperationException("Unauthorized or tournament is not finished.");
            }

            // Verify the selected winner actually joined this tournament
            if (!tournament.PlayersTournaments.Any(pt => pt.PlayerId == winnerId))
            {
                throw new InvalidOperationException("The winner must be a registered player in this tournament.");
            }

            tournament.WinnerId = winnerId;
            await context.SaveChangesAsync();
        }

    }
}
