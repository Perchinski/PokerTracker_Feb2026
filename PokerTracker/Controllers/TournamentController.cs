using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PokerTracker.GCommon;
using PokerTracker.Services.Core;
using PokerTracker.Services.Core.Contracts;
using PokerTracker.ViewModels.Tournaments;
using System.Security.Claims;

namespace PokerTracker.Controllers
{
    /// <summary>
    /// Handles all tournament-related HTTP requests including listing, creating, joining, and managing tournament lifecycles.
    /// </summary>
    [Authorize]
    public class TournamentController(ITournamentService tournamentService, ILocationService locationService, ILogger<TournamentController> logger) : BaseController
    {
        private bool IsAdmin => User.IsInRole(ApplicationConstants.Roles.Administrator);

        /// <summary>
        /// Displays a paginated and filtered list of tournaments available to the authenticated user.
        /// </summary>
        /// <param name="searchTerm">A search string matching the tournament name or description.</param>
        /// <param name="formatId">The ID of the tournament format to filter by.</param>
        /// <param name="status">The current status of the tournament (e.g., Upcoming, Running, Finished).</param>
        /// <param name="onlyJoined">Indicates whether to show only tournaments the current user has joined.</param>
        /// <param name="onlyOwned">Indicates whether to show only tournaments created by the current user.</param>
        /// <param name="sortOrder">Determines how the tournaments are sorted.</param>
        /// <param name="pageNumber">The numeric page of results to display.</param>
        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, int? formatId, string? status, bool onlyJoined, bool onlyOwned, string sortOrder = ApplicationConstants.SortOrders.StatusDefault, int pageNumber = 1)
        {
            logger.LogInformation("Index method called in TournamentController");

            const int pageSize = ApplicationConstants.DefaultPageSize;
            string? userId = GetUserId();
            var (tournaments, totalCount) = await tournamentService.GetAllTournamentsAsync(searchTerm, formatId, status, sortOrder, onlyJoined, onlyOwned, userId, IsAdmin, pageNumber, pageSize);

            var formats = await tournamentService.GetFormatsAsync();

            var model = new TournamentFilterViewModel
            {
                Tournaments = tournaments,
                SearchTerm = searchTerm,
                FormatId = formatId,
                Status = status, 
                SortOrder = sortOrder,
                Formats = formats,
                OnlyJoined = onlyJoined,
                OnlyOwned = onlyOwned,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return View(model);
        }

        /// <summary>
        /// Displays the view to create a new tournament.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new TournamentFormModel
            {
                // Pre-load the formats and locations for the dropdown
                Formats = await tournamentService.GetFormatsAsync(),
                Locations = await tournamentService.GetActiveLocationsAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TournamentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Formats = await tournamentService.GetFormatsAsync();
                model.Locations = await tournamentService.GetActiveLocationsAsync();
                return View(model);
            }

            try
            {
                string? userId = GetUserId();

                await tournamentService.CreateAsync(model, userId);

                return RedirectToAction("Index", "Tournament");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating the tournament for user {UserId}", GetUserId());
                ModelState.AddModelError("", "Something went wrong while creating the tournament.");
                model.Formats = await tournamentService.GetFormatsAsync();
                model.Locations = await tournamentService.GetActiveLocationsAsync();
                return View(model);
            }
        }

        /// <summary>
        /// Displays the detailed view of a specific tournament. Includes players, format data, and location details.
        /// </summary>
        /// <param name="id">The unique identifier of the tournament.</param>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            string? userId = GetUserId();

            var model = await tournamentService.GetDetailsAsync(id, userId, IsAdmin);

            if (model == null)
            {
                logger.LogWarning("Tournament with ID {TournamentId} was not found when accessed by user {UserId}", id, userId);
                return NotFound();
            }

            return View(model);
        }

        /// <summary>
        /// Transitions a tournament from 'Upcoming' to 'Running' status.
        /// </summary>
        /// <param name="id">The unique identifier of the tournament.</param>
        [HttpPost]
        public async Task<IActionResult> Start(int id)
        {
            string? userId = GetUserId();
            try
            {
                await tournamentService.StartAsync(id, userId, IsAdmin);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error starting tournament {TournamentId} for user {UserId}", id, userId);
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        /// <summary>
        /// Transitions a tournament from 'Running' to 'Finished' status.
        /// </summary>
        /// <param name="id">The unique identifier of the tournament.</param>
        [HttpPost]
        public async Task<IActionResult> Finish(int id)
        {
            string? userId = GetUserId();
            try
            {
                await tournamentService.FinishAsync(id, userId, IsAdmin);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error finishing tournament {TournamentId} for user {UserId}", id, userId);
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        /// <summary>
        /// Registers the currently authenticated user as a player in the specified tournament.
        /// </summary>
        /// <param name="id">The unique identifier of the tournament.</param>
        [HttpPost]
        public async Task<IActionResult> Join(int id)
        {
            string? userId = GetUserId();

            if (userId == null)
            {
                return Challenge();
            }

            try
            {
                await tournamentService.JoinAsync(id, userId);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "User {UserId} failed to join tournament {TournamentId}", userId, id);
                TempData["ErrorMessage"] = "Unable to join the tournament. " + ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        /// <summary>
        /// Unregisters the currently authenticated user from the specified tournament.
        /// </summary>
        /// <param name="id">The unique identifier of the tournament.</param>
        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            string? userId = GetUserId();

            if (userId == null)
            {
                return Challenge();
            }

            try
            {
                await tournamentService.LeaveAsync(id, userId);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "User {UserId} failed to leave tournament {TournamentId}", userId, id);
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> RemovePlayer(int id, string playerId)
        {
            string? userId = GetUserId();

            if (userId == null)
            {
                return Challenge();
            }

            try
            {
                await tournamentService.RemovePlayerAsync(id, playerId, userId, IsAdmin);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to remove player {PlayerId} from tournament {TournamentId} by user {UserId}", playerId, id, userId);
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        /// <summary>
        /// Displays the view to edit an existing tournament. Ensures the user is the owner or an admin.
        /// </summary>
        /// <param name="id">The unique identifier of the tournament.</param>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string? userId = GetUserId();

            var model = await tournamentService.GetForEditAsync(id, userId, IsAdmin);

            if (model == null)
            {
                // If null they aren't the owner
                logger.LogWarning("User {UserId} attempted to edit tournament {TournamentId} but was forbidden (not owner or not found)", userId, id);
                return Forbid();
            }

            model.Formats = await tournamentService.GetFormatsAsync();
            model.Locations = await tournamentService.GetActiveLocationsAsync();

            return View(model);
        }

        /// <summary>
        /// Processes the submission of an edited tournament form.
        /// </summary>
        /// <param name="id">The unique identifier of the tournament being edited.</param>
        /// <param name="model">The tournament form data containing edits.</param>
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TournamentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Formats = await tournamentService.GetFormatsAsync();
                model.Locations = await tournamentService.GetActiveLocationsAsync();
                return View(model);
            }

            string? userId = GetUserId();

            try
            {
                await tournamentService.EditAsync(id, model, userId, IsAdmin);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Update failed for tournament {TournamentId} by user {UserId}", id, userId);
                ModelState.AddModelError(string.Empty, "Update failed: " + ex.Message);

                model.Formats = await tournamentService.GetFormatsAsync();
                model.Locations = await tournamentService.GetActiveLocationsAsync();

                return View(model);
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        /// <summary>
        /// Displays a confirmation view prior to deleting a tournament.
        /// </summary>
        /// <param name="id">The unique identifier of the tournament.</param>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string? userId = GetUserId();

            var tournament = await tournamentService.GetDetailsAsync(id, userId, IsAdmin);

            if (tournament == null)
            {
                logger.LogWarning("Tournament with ID {TournamentId} was not found when user {UserId} tried to view delete confirm", id, userId);
                return NotFound();
            }

            if (!tournament.IsOwner)
            {
                return Forbid();
            }
            return View(tournament);
        }

        /// <summary>
        /// Processes the deletion of a specific tournament after confirmation.
        /// </summary>
        /// <param name="id">The unique identifier of the tournament to be deleted.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string? userId = GetUserId();

            try
            {
                await tournamentService.DeleteAsync(id, userId, IsAdmin);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete tournament {TournamentId} by user {UserId}", id, userId);
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Displays the view allowing the creator (or admin) to select a winner for a finished tournament.
        /// </summary>
        /// <param name="id">The unique identifier of the tournament.</param>
        [HttpGet]
        public async Task<IActionResult> SelectWinner(int id)
        {
            string? userId = GetUserId();

            var tournament = await tournamentService.GetDetailsAsync(id, userId, IsAdmin);
            if (tournament == null)
            {
                logger.LogWarning("Tournament with ID {TournamentId} was not found when user {UserId} tried to select a winner", id, userId);
                return NotFound();
            }
            if (!tournament.IsOwner)
            {
                return Forbid();
            }
            if (tournament.Status != "Finished")
            {
                TempData["ErrorMessage"] = "You cannot select a winner for a tournament that is still in progress.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var model = new SelectWinnerViewModel
            {
                TournamentId = tournament.Id,
                TournamentName = tournament.Name,
                Players = tournament.Players.Select(p => new PlayerViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
            };

            return View(model);
        }

        /// <summary>
        /// Submits the chosen winner for a given finished tournament.
        /// </summary>
        /// <param name="id">The unique identifier of the tournament.</param>
        /// <param name="model">The view model containing the selected winner's ID.</param>
        [HttpPost]
        public async Task<IActionResult> SelectWinner(int id, SelectWinnerViewModel model)
        {
            string? userId = GetUserId();

            if (!ModelState.IsValid)
            {
                var tournament = await tournamentService.GetDetailsAsync(id, userId, IsAdmin);
                model.Players = tournament!.Players.Select(p => new PlayerViewModel { Id = p.Id, Name = p.Name });
                return View(model);
            }

            try
            {
                await tournamentService.SetWinnerAsync(id, model.WinnerId, userId, IsAdmin);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Could not set winner {WinnerId} for tournament {TournamentId} by user {UserId}", model.WinnerId, id, userId);
                ModelState.AddModelError("", "Could not set winner: " + ex.Message);

                var tournament = await tournamentService.GetDetailsAsync(id, userId);
                model.Players = tournament.Players.Select(p => new PlayerViewModel { Id = p.Id, Name = p.Name });

                return View(model);
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        /// <summary>
        /// Displays the details of a specific location linked to a tournament.
        /// </summary>
        /// <param name="locationId">The identifier of the location.</param>
        /// <param name="tournamentId">Optional identifier to link back to the tournament details.</param>
        [HttpGet]
        public async Task<IActionResult> LocationDetails(int locationId, int? tournamentId)
        {
            var location = await locationService.GetLocationDetailsAsync(locationId);

            if (location == null)
            {
                logger.LogWarning("Location with ID {LocationId} was not found", locationId);
                return NotFound();
            }

            ViewBag.TournamentId = tournamentId;

            return View(location);
        }
    }
}
