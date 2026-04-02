using Microsoft.AspNetCore.Mvc;
using PokerTracker.Services.Core.Contracts;
using PokerTracker.ViewModels.Admin.Locations;

namespace PokerTracker.Areas.Admin.Controllers
{
    public class LocationController : BaseAdminController
    {
        private readonly ILocationService locationService;
        private readonly ILogger<LocationController> logger;

        public LocationController(ILocationService locationService, ILogger<LocationController> logger)
        {
            this.locationService = locationService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var locations = await locationService.GetAllLocationsAsync();
            return View(locations);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new LocationFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(LocationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await locationService.CreateLocationAsync(model);
                TempData["Success"] = "Location created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating location.");
                ModelState.AddModelError("", "An error occurred while creating the location.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await locationService.GetLocationForEditAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, LocationFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await locationService.UpdateLocationAsync(id, model);
                TempData["Success"] = "Location updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating location {LocationId}", id);
                ModelState.AddModelError("", "An error occurred while updating the location.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await locationService.DeleteLocationAsync(id);
                TempData["Success"] = "Location and associated tournaments deleted successfully.";
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting location {LocationId}", id);
                TempData["Error"] = "An error occurred while deleting the location.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
