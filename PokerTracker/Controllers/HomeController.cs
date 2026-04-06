using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokerTracker.ViewModels;

namespace PokerTracker.Controllers
{
    // Overrides BaseController's [Authorize] — public pages don't require login
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> logger;
        private readonly PokerTracker.Services.Core.Contracts.IAnnouncementService announcementService;

        public HomeController(ILogger<HomeController> logger, PokerTracker.Services.Core.Contracts.IAnnouncementService announcementService)
        {
            this.logger = logger;
            this.announcementService = announcementService;
        }

        /// <summary>
        /// Displays the landing page for the application.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var announcements = await announcementService.GetAllAsync(includeInactive: false);
            return View(announcements);
        }

        /// <summary>
        /// Displays the privacy policy statement.
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Handles global error display and logging for unexpected exceptions and status codes.
        /// </summary>
        /// <param name="statusCode">The HTTP status code resulting in the error.</param>
        /// <returns>A specific error view based on the HTTP status code.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            var exceptionDetails = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

            if (exceptionDetails != null)
            {
                logger.LogError(exceptionDetails.Error, "An unhandled exception occurred during request execution to {Path}", exceptionDetails.Path);
                
                if (!statusCode.HasValue)
                {
                    statusCode = StatusCodes.Status500InternalServerError;
                }
            }

            switch (statusCode)
            {
                case StatusCodes.Status404NotFound:
                    logger.LogWarning("404 Not Found error occurred. Original Path: {Path}", HttpContext.Request.Path);
                    return View("Errors/Error404");
                case StatusCodes.Status500InternalServerError:
                    return View("Errors/Error500");
                case StatusCodes.Status403Forbidden:
                    logger.LogWarning("403 Forbidden error occurred. Original Path: {Path}", HttpContext.Request.Path);
                    return View("Errors/Error403");
                case StatusCodes.Status400BadRequest:
                    logger.LogWarning("400 Bad Request error occurred. Original Path: {Path}", HttpContext.Request.Path);
                    return View("Errors/Error400");
            }

            return View("Errors/Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenied()
        {
            return View("Errors/Error403");
        }
    }
}
