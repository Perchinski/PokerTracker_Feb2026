using Microsoft.AspNetCore.Mvc;
using PokerTracker.Services.Core.Contracts;
using PokerTracker.ViewModels.Announcements;

namespace PokerTracker.Areas.Admin.Controllers;

public class AnnouncementController : BaseAdminController
{
    private readonly IAnnouncementService announcementService;
    private readonly ILogger<AnnouncementController> logger;

    public AnnouncementController(IAnnouncementService announcementService, ILogger<AnnouncementController> logger)
    {
        this.announcementService = announcementService;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var announcements = await announcementService.GetAllAsync(includeInactive: true);
        return View(announcements);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new AnnouncementViewModel { IsActive = true, PublishedOn = DateTime.UtcNow });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AnnouncementViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await announcementService.CreateAsync(model);
            TempData["Success"] = "Announcement created successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating announcement");
            ModelState.AddModelError("", "An unexpected error occurred.");
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await announcementService.GetByIdAsync(id);
        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AnnouncementViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await announcementService.UpdateAsync(model);
            TempData["Success"] = "Announcement updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating announcement");
            ModelState.AddModelError("", "An unexpected error occurred.");
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await announcementService.DeleteAsync(id);
            TempData["Success"] = "Announcement deleted successfully!";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting announcement");
            TempData["Error"] = "Could not delete the announcement.";
        }

        return RedirectToAction(nameof(Index));
    }
}
