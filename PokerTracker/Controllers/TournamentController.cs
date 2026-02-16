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
        public async Task<IActionResult> Index()
        {
            var tournaments = await tournamentService.GetAllTournamentsAsync();
            return View(tournaments);
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
            await tournamentService.StartAsync(id, userId);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Finish(int id)
        {
            string? userId = GetUserId();
            await tournamentService.FinishAsync(id, userId);
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

            await tournamentService.LeaveAsync(id, userId);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string? userId = GetUserId();

            var model = await tournamentService.GetForEditAsync(id, userId);

            if (model == null)
            {
                // If null, either it doesn't exist or they aren't the owner
                return Unauthorized();
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
            catch (Exception)
            {
                return Unauthorized();
            }

            // Redirect back to the details page to see changes
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string? userId = GetUserId();

            var tournament = await tournamentService.GetDetailsAsync(id, userId);

            if (tournament == null || !tournament.IsOwner)
            {
                return Unauthorized();
            }

            return View(tournament);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string? userId = GetUserId();

            await tournamentService.DeleteAsync(id, userId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SelectWinner(int id)
        {
            string? userId = GetUserId();

            var tournament = await tournamentService.GetDetailsAsync(id, userId);

            if (tournament == null || !tournament.IsOwner || tournament.Status != "Finished")
            {
                return Unauthorized();
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
                // If validation fails, redirect back to try again
                return RedirectToAction(nameof(SelectWinner), new { id });
            }

            try
            {
                await tournamentService.SetWinnerAsync(id, model.WinnerId, userId);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
