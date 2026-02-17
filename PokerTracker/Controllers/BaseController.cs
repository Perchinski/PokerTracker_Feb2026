using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PokerTracker.Controllers
{
    /// <summary>
    /// Base controller — enforces authentication and CSRF protection for all derived controllers.
    /// </summary>
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class BaseController : Controller
    {
        protected string? GetUserId()
        {
            return User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
