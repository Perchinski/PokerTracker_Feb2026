using Microsoft.AspNetCore.Mvc;

namespace PokerTracker.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
