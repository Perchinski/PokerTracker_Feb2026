using Microsoft.AspNetCore.Mvc;
using PokerTracker.Services.Core.Contracts;
using PokerTracker.ViewModels.Admin.Locations;

namespace PokerTracker.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin controller responsible for managing locations used for tournaments.
    /// </summary>
    public class LocationController : BaseAdminController
    {
        private readonly ILocationService locationService;
        private readonly ILogger<LocationController> logger;

        public LocationController(ILocationService locationService, ILogger<LocationController> logger)
        {
            this.locationService = locationService;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves and displays a list of all locations for administrative purposes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var locations = await locationService.GetAllLocationsAsync();
            return View(locations);
        }

        /// <summary>
        /// Displays the view to create a newly tracked location.
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new LocationFormViewModel());
        }

        /// <summary>
        /// Processes the creation of a new location.
        /// </summary>
        /// <param name="model">Data from the submitted form representing the new location.</param>
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

        /// <summary>
        /// Displays the view to modify an existing location's properties.
        /// </summary>
        /// <param name="id">The unique identifier of the location to load.</param>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await locationService.GetLocationForEditAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        /// <summary>
        /// Processes the updates to an existing location.
        /// </summary>
        /// <param name="id">The unique identifier of the location being changed.</param>
        /// <param name="model">The values submitted to be applied to the location.</param>
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

        /// <summary>
        /// Deletes a location and associated tournaments from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the location to remove.</param>
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
