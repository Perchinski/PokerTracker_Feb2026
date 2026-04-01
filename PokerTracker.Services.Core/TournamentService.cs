using Microsoft.EntityFrameworkCore;
using PokerTracker.Data.Models;
using PokerTracker.Data.Repository.Contracts;
using PokerTracker.Services.Core.Contracts;
using PokerTracker.ViewModels.Tournaments;

namespace PokerTracker.Services.Core
{
    public class TournamentService(ITournamentRepository repository) : ITournamentService
    {
        public async Task<IEnumerable<TournamentFormatViewModel>> GetFormatsAsync()
        {
            return await repository.GetAllFormatsQuery()
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
            if (!await repository.FormatExistsAsync(model.FormatId))
            {
                throw new ArgumentException("Invalid tournament format.");
            }

            var tournament = new Tournament
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Date = model.Date,
                FormatId = model.FormatId,
                CreatorId = userId
            };

            await repository.AddAsync(tournament);
            await repository.SaveChangesAsync();
        }

        public async Task<List<TournamentIndexViewModel>> GetAllTournamentsAsync(
            string? searchTerm, int? formatId, string? status, string sortOrder, bool onlyJoined, bool onlyOwned, string? userId)
        {
            var query = repository.GetAllTournamentsQuery();

            // Filtering
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

            if (onlyOwned && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(t => t.CreatorId == userId);
            }

            if (formatId.HasValue)
            {
                query = query.Where(t => t.FormatId == formatId.Value);
            }

            if (!string.IsNullOrEmpty(status) && Enum.TryParse(typeof(TournamentStatus), status, out var statusEnum))
            {
                query = query.Where(t => t.Status == (TournamentStatus)statusEnum);
            }

            // Sorting
            query = sortOrder switch
            {
                "date_asc" => query.OrderBy(t => t.Date),
                "date_desc" => query.OrderByDescending(t => t.Date),
                "name_asc" => query.OrderBy(t => t.Name),
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
            var tournament = await repository.GetByIdAsync(id);
            ValidateOwnershipAndStatus(tournament, userId, TournamentStatus.Open, "Only the creator can start an open tournament.");

            tournament!.Status = TournamentStatus.Running;
            await repository.SaveChangesAsync();
        }

        public async Task FinishAsync(int id, string userId)
        {
            var tournament = await repository.GetByIdAsync(id);
            ValidateOwnershipAndStatus(tournament, userId, TournamentStatus.Running, "Only the creator can finish a running tournament.");

            tournament!.Status = TournamentStatus.Finished;
            await repository.SaveChangesAsync();
        }

        public async Task<TournamentDetailsViewModel?> GetDetailsAsync(int id, string? userId)
        {
            var tournament = await repository.GetDetailsByIdAsync(id);

            if (tournament == null) return null;

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
            var tournament = await repository.GetByIdWithPlayersAsync(tournamentId);

            if (tournament == null) throw new ArgumentException("Tournament not found");
            if (tournament.Status != TournamentStatus.Open) throw new InvalidOperationException("Tournament is not open.");
            if (tournament.PlayersTournaments.Any(pt => pt.PlayerId == userId)) throw new InvalidOperationException("Already joined.");

            tournament.PlayersTournaments.Add(new PlayerTournament { TournamentId = tournamentId, PlayerId = userId });
            await repository.SaveChangesAsync();
        }

        public async Task LeaveAsync(int tournamentId, string userId)
        {
            var tournament = await repository.GetByIdWithPlayersAsync(tournamentId);

            if (tournament == null || tournament.Status != TournamentStatus.Open)
                throw new InvalidOperationException("Cannot leave this tournament.");

            var pt = tournament.PlayersTournaments.FirstOrDefault(x => x.PlayerId == userId);
            if (pt == null) throw new InvalidOperationException("Not registered.");

            tournament.PlayersTournaments.Remove(pt);
            await repository.SaveChangesAsync();
        }

        public async Task<TournamentFormModel?> GetForEditAsync(int id, string userId)
        {
            var tournament = await repository.GetByIdAsync(id);
            if (tournament == null || tournament.CreatorId != userId) return null;

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
            var tournament = await repository.GetByIdAsync(id);
            ValidateOwnershipAndStatus(tournament, userId, TournamentStatus.Open, "Cannot edit this tournament.");

            if (!await repository.FormatExistsAsync(model.FormatId)) throw new ArgumentException("Invalid format.");

            tournament!.Name = model.Name;
            tournament.Description = model.Description;
            tournament.Date = model.Date;
            tournament.FormatId = model.FormatId;
            tournament.ImageUrl = model.ImageUrl;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var tournament = await repository.GetByIdAsync(id);
            if (tournament == null || tournament.CreatorId != userId) throw new InvalidOperationException("Unauthorized.");

            tournament.IsDeleted = true;
            await repository.SaveChangesAsync();
        }

        public async Task SetWinnerAsync(int tournamentId, string winnerId, string userId)
        {
            var tournament = await repository.GetByIdWithPlayersAsync(tournamentId);
            ValidateOwnershipAndStatus(tournament, userId, TournamentStatus.Finished, "Unauthorized or tournament not finished.");

            if (!tournament!.PlayersTournaments.Any(pt => pt.PlayerId == winnerId))
                throw new InvalidOperationException("Winner must be a player.");

            tournament.WinnerId = winnerId;
            await repository.SaveChangesAsync();
        }

        public async Task DeleteUserRelatedDataAsync(string userId)
        {
            await repository.RemoveUserRelatedDataAsync(userId);
            await repository.SaveChangesAsync();
        }

        private void ValidateOwnershipAndStatus(Tournament? t, string uid, TournamentStatus s, string msg)
        {
            if (t == null || t.CreatorId != uid || t.Status != s) throw new InvalidOperationException(msg);
        }
    }
}