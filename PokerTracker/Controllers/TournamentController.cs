using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PokerTracker.Services.Core;
using PokerTracker.Services.Core.Contracts;
using PokerTracker.ViewModels.Tournaments;
using System.Security.Claims;

namespace PokerTracker.Controllers
{
    [Authorize]
    public class TournamentController(ITournamentService tournamentService) : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> Index(string? searchTerm, int? formatId, string? status, bool onlyJoined, bool onlyOwned, string sortOrder = "status")
        {
            string? userId = GetUserId();
            var tournaments = await tournamentService.GetAllTournamentsAsync(searchTerm, formatId, status, sortOrder, onlyJoined, onlyOwned, userId);

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
                OnlyOwned = onlyOwned
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new TournamentFormModel
            {
                // Pre-load the formats for the dropdown
                Formats = await tournamentService.GetFormatsAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TournamentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Formats = await tournamentService.GetFormatsAsync();
                return View(model);
            }

            try
            {
                string? userId = GetUserId();

                await tournamentService.CreateAsync(model, userId);

                return RedirectToAction("Index", "Tournament");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong while creating the tournament.");
                model.Formats = await tournamentService.GetFormatsAsync();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            string? userId = GetUserId();

            var model = await tournamentService.GetDetailsAsync(id, userId);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Start(int id)
        {
            string? userId = GetUserId();
            try
            {
                await tournamentService.StartAsync(id, userId);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Finish(int id)
        {
            string? userId = GetUserId();
            try
            {
                await tournamentService.FinishAsync(id, userId);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

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
                TempData["ErrorMessage"] = "Unable to join the tournament. " + ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

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
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string? userId = GetUserId();

            var model = await tournamentService.GetForEditAsync(id, userId);

            if (model == null)
            {
                // If null they aren't the owner
                return Forbid();
            }

            model.Formats = await tournamentService.GetFormatsAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TournamentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Formats = await tournamentService.GetFormatsAsync();
                return View(model);
            }

            string? userId = GetUserId();

            try
            {
                await tournamentService.EditAsync(id, model, userId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Update failed: " + ex.Message);

                model.Formats = await tournamentService.GetFormatsAsync();

                return View(model);
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string? userId = GetUserId();

            var tournament = await tournamentService.GetDetailsAsync(id, userId);

            if (tournament == null)
            {
                return NotFound();
            }

            if (!tournament.IsOwner)
            {
                return Forbid();
            }
            return View(tournament);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string? userId = GetUserId();

            try
            {
                await tournamentService.DeleteAsync(id, userId);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SelectWinner(int id)
        {
            string? userId = GetUserId();

            var tournament = await tournamentService.GetDetailsAsync(id, userId);
            if (tournament == null)
            {
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

        [HttpPost]
        public async Task<IActionResult> SelectWinner(int id, SelectWinnerViewModel model)
        {
            string? userId = GetUserId();

            if (!ModelState.IsValid)
            {
                var tournament = await tournamentService.GetDetailsAsync(id, userId);
                model.Players = tournament.Players.Select(p => new PlayerViewModel { Id = p.Id, Name = p.Name });
                return View(model);
            }

            try
            {
                await tournamentService.SetWinnerAsync(id, model.WinnerId, userId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Could not set winner: " + ex.Message);

                var tournament = await tournamentService.GetDetailsAsync(id, userId);
                model.Players = tournament.Players.Select(p => new PlayerViewModel { Id = p.Id, Name = p.Name });

                return View(model);
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
