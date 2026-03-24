using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokerTracker.ViewModels.Admin;

namespace PokerTracker.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        private readonly UserManager<IdentityUser> userManager;

        // Inject the Identity UserManager
        public UserController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Get all users from the database
            var users = await userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            // 2. Loop through and grab their roles
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? "No Email",
                    Roles = roles
                });
            }

            // 3. Send the data to the view
            return View(userViewModels);
        }
    }
}