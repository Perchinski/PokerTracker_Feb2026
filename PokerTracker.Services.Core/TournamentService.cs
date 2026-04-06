using Microsoft.EntityFrameworkCore;
using PokerTracker.Data.Models;
using PokerTracker.Data.Repository.Contracts;
using PokerTracker.GCommon;
using PokerTracker.Services.Core.Contracts;
using PokerTracker.ViewModels.Tournaments;

namespace PokerTracker.Services.Core
{
    /// <summary>
    /// Responsible for the primary business logic regarding tournament setups, active tracking, participant management, and validation. 
    /// Inherits method contracts from <see cref="ITournamentService"/>.
    /// </summary>
    public class TournamentService(ITournamentRepository repository) : ITournamentService
    {
        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<IEnumerable<LocationSelectionViewModel>> GetActiveLocationsAsync()
        {
            return await repository.GetActiveLocationsQuery()
                .Select(l => new LocationSelectionViewModel
                {
                    Id = l.Id,
                    Name = l.Name,
                    City = l.City
                })
                .AsNoTracking()
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task CreateAsync(TournamentFormModel model, string userId)
        {
            if (!await repository.FormatExistsAsync(model.FormatId))
            {
                throw new ArgumentException("Invalid tournament format.");
            }

            if (!await repository.LocationExistsAsync(model.LocationId))
            {
                throw new ArgumentException("Invalid location.");
            }

            var tournament = new Tournament
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Date = model.Date,
                FormatId = model.FormatId,
                LocationId = model.LocationId,
                CreatorId = userId
            };

            await repository.AddAsync(tournament);
            await repository.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<(List<TournamentIndexViewModel> Tournaments, int TotalCount)> GetAllTournamentsAsync(
            string? searchTerm, int? formatId, string? status, string sortOrder, bool onlyJoined, bool onlyOwned, string? userId, bool isAdmin = false, int pageNumber = 1, int pageSize = ApplicationConstants.DefaultPageSize)
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
                ApplicationConstants.SortOrders.DateAscending => query.OrderBy(t => t.Date),
                ApplicationConstants.SortOrders.DateDescending => query.OrderByDescending(t => t.Date),
                ApplicationConstants.SortOrders.NameAscending => query.OrderBy(t => t.Name),
                _ => query.OrderBy(t => t.Status).ThenBy(t => t.Date)
            };

            int totalCount = await query.CountAsync();

            var tournaments = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TournamentIndexViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Date = t.Date,
                    Format = t.Format.Name,
                    LocationName = t.Location.Name,
                    Status = t.Status.ToString(),
                    PlayersCount = t.PlayersTournaments.Count,
                    ImageUrl = t.ImageUrl,
                    Creator = t.Creator.UserName ?? "Unknown",
                    IsJoined = userId != null && t.PlayersTournaments.Any(pt => pt.PlayerId == userId),
                    IsOwner = isAdmin || t.CreatorId == userId,
                    IsCreator = t.CreatorId == userId,
                })
                .ToListAsync();

            return (tournaments, totalCount);
        }

        /// <inheritdoc/>
        public async Task StartAsync(int id, string userId, bool isAdmin = false)
        {
            var tournament = await repository.GetByIdAsync(id);
            ValidateOwnershipAndStatus(tournament, userId, isAdmin, TournamentStatus.Open, "Only the creator or an admin can start an open tournament.");

            tournament!.Status = TournamentStatus.Running;
            await repository.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task FinishAsync(int id, string userId, bool isAdmin = false)
        {
            var tournament = await repository.GetByIdAsync(id);
            ValidateOwnershipAndStatus(tournament, userId, isAdmin, TournamentStatus.Running, "Only the creator or an admin can finish a running tournament.");

            tournament!.Status = TournamentStatus.Finished;
            await repository.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<TournamentDetailsViewModel?> GetDetailsAsync(int id, string? userId, bool isAdmin = false)
        {
            var tournament = await repository.GetDetailsByIdAsync(id);

            if (tournament == null) return null;

            return new TournamentDetailsViewModel
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description ?? "No description provided.",
                Format = tournament.Format.Name,
                LocationId = tournament.LocationId,
                LocationName = tournament.Location.Name,
                LocationAddress = tournament.Location.Address,
                LocationCity = tournament.Location.City,
                LocationImageUrl = tournament.Location.ImageUrl,
                LocationLatitude = tournament.Location.Latitude,
                LocationLongitude = tournament.Location.Longitude,
                Creator = tournament.Creator.UserName ?? "Unknown",
                Date = tournament.Date,
                ImageUrl = tournament.ImageUrl,
                Status = tournament.Status.ToString(),
                IsJoined = userId != null && tournament.PlayersTournaments.Any(pt => pt.PlayerId == userId),
                IsOwner = isAdmin || tournament.CreatorId == userId,
                IsCreator = tournament.CreatorId == userId,
                WinnerName = tournament.Winner?.UserName,
                Players = tournament.PlayersTournaments.Select(pt => new PlayerViewModel
                {
                    Id = pt.PlayerId,
                    Name = pt.Player.UserName ?? "Unknown Player"
                }).ToList()
            };
        }

        /// <inheritdoc/>
        public async Task JoinAsync(int tournamentId, string userId)
        {
            var tournament = await repository.GetByIdWithPlayersAsync(tournamentId);

            if (tournament == null) throw new ArgumentException("Tournament not found");
            if (tournament.Status != TournamentStatus.Open) throw new InvalidOperationException("Tournament is not open.");
            if (tournament.PlayersTournaments.Any(pt => pt.PlayerId == userId)) throw new InvalidOperationException("Already joined.");

            tournament.PlayersTournaments.Add(new PlayerTournament { TournamentId = tournamentId, PlayerId = userId });
            await repository.SaveChangesAsync();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task RemovePlayerAsync(int tournamentId, string playerIdToRemove, string currentUserId, bool isAdmin = false)
        {
            var tournament = await repository.GetByIdWithPlayersAsync(tournamentId);

            if (tournament == null) throw new InvalidOperationException("Tournament not found.");

            if (!isAdmin && tournament.CreatorId != currentUserId)
            {
                throw new InvalidOperationException("Only the owner or an admin can remove players.");
            }

            if (tournament.Status != TournamentStatus.Open && tournament.Status != TournamentStatus.Running) 
            {
                throw new InvalidOperationException("Players can only be removed while the tournament is open or running.");
            }

            var pt = tournament.PlayersTournaments.FirstOrDefault(x => x.PlayerId == playerIdToRemove);
            if (pt == null) throw new InvalidOperationException("Player is not registered.");

            tournament.PlayersTournaments.Remove(pt);
            await repository.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<TournamentFormModel?> GetForEditAsync(int id, string userId, bool isAdmin = false)
        {
            var tournament = await repository.GetByIdAsync(id);
            if (tournament == null || (!isAdmin && tournament.CreatorId != userId)) return null;

            return new TournamentFormModel
            {
                Name = tournament.Name,
                Description = tournament.Description,
                Date = tournament.Date,
                FormatId = tournament.FormatId,
                LocationId = tournament.LocationId,
                ImageUrl = tournament.ImageUrl
            };
        }

        /// <inheritdoc/>
        public async Task EditAsync(int id, TournamentFormModel model, string userId, bool isAdmin = false)
        {
            var tournament = await repository.GetByIdAsync(id);
            if (tournament == null) throw new InvalidOperationException("Cannot edit this tournament.");
            if (!isAdmin && tournament.CreatorId != userId) throw new InvalidOperationException("Cannot edit this tournament.");

            if (!await repository.FormatExistsAsync(model.FormatId)) throw new ArgumentException("Invalid format.");
            if (!await repository.LocationExistsAsync(model.LocationId)) throw new ArgumentException("Invalid location. (It might be closed or archived).");

            tournament!.Name = model.Name;
            tournament.Description = model.Description;
            tournament.Date = model.Date;
            tournament.FormatId = model.FormatId;
            tournament.LocationId = model.LocationId;
            tournament.ImageUrl = model.ImageUrl;

            await repository.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id, string userId, bool isAdmin = false)
        {
            var tournament = await repository.GetByIdAsync(id);
            if (tournament == null || (!isAdmin && tournament.CreatorId != userId)) throw new InvalidOperationException("Unauthorized.");

            tournament.IsDeleted = true;
            await repository.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task SetWinnerAsync(int tournamentId, string winnerId, string userId, bool isAdmin = false)
        {
            var tournament = await repository.GetByIdWithPlayersAsync(tournamentId);
            ValidateOwnershipAndStatus(tournament, userId, isAdmin, TournamentStatus.Finished, "Unauthorized or tournament not finished.");

            if (!tournament!.PlayersTournaments.Any(pt => pt.PlayerId == winnerId))
                throw new InvalidOperationException("Winner must be a player.");

            tournament.WinnerId = winnerId;
            await repository.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task DeleteUserRelatedDataAsync(string userId)
        {
            await repository.RemoveUserRelatedDataAsync(userId);
            await repository.SaveChangesAsync();
        }

        private void ValidateOwnershipAndStatus(Tournament? t, string uid, bool isAdmin, TournamentStatus s, string msg)
        {
            if (t == null) throw new InvalidOperationException(msg);
            if (!isAdmin && t.CreatorId != uid) throw new InvalidOperationException(msg);
            if (t.Status != s) throw new InvalidOperationException(msg);
        }
    }
}