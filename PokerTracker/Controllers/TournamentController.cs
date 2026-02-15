using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

                return RedirectToAction("Index", "Home");
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

    }
}
