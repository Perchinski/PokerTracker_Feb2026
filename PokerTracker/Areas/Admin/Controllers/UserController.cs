using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokerTracker.Services.Core.Contracts;
using PokerTracker.ViewModels.Admin.Users;

namespace PokerTracker.Areas.Admin.Controllers
{
    /// <summary>
    /// Administrative controller for managing registered user accounts and their associated roles.
    /// </summary>
    public class UserController : BaseAdminController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITournamentService tournamentService;
        private readonly ILogger<UserController> logger;

        public UserController(
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            ITournamentService tournamentService,
            ILogger<UserController> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tournamentService = tournamentService;
            this.logger = logger;
        }

        /// <summary>
        /// Displays a list of all registered users with their assigned roles.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? "No Email",
                    Roles = await userManager.GetRolesAsync(user)
                });
            }

            return View(userViewModels);
        }

        /// <summary>
        /// Displays a form allowing an admin to manage a user's assigned roles.
        /// </summary>
        /// <param name="id">The unique string identity of the user.</param>
        [HttpGet]
        public async Task<IActionResult> EditRoles(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var model = new UserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty
            };

            foreach (var role in roleManager.Roles.ToList())
            {
                model.Roles.Add(new RoleSelectionViewModel
                {
                    RoleName = role.Name ?? string.Empty,
                    IsSelected = await userManager.IsInRoleAsync(user, role.Name!)
                });
            }

            return View(model);
        }

        /// <summary>
        /// Applies the selected role changes for a given user, ensuring the admin does not remove their own access.
        /// </summary>
        /// <param name="model">The submitted user role data holding the changes.</param>
        [HttpPost]
        public async Task<IActionResult> EditRoles(UserRolesViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            // Prevent the logged-in admin from accidentally removing their own admin privileges
            if (user.Id == GetAdminUserId() && !model.Roles.Any(r => r.RoleName == PokerTracker.GCommon.ApplicationConstants.Roles.Administrator && r.IsSelected))
            {
                TempData["Error"] = "You cannot remove your own Administrator role.";
                return RedirectToAction(nameof(Index));
            }

            var currentRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, currentRoles);

            var selectedRoles = model.Roles.Where(x => x.IsSelected).Select(y => y.RoleName);
            await userManager.AddToRolesAsync(user, selectedRoles);

            TempData["Success"] = "User roles updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Deletes a user account and purges any associated content, such as their hosted tournaments.
        /// </summary>
        /// <param name="id">The unique string identity of the user to be deleted.</param>
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // Prevent admins from deleting themselves
            if (user.Id == GetAdminUserId())
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Remove this user from any tournaments they joined as a player, and clean up their hosted tournaments
                await tournamentService.DeleteUserRelatedDataAsync(user.Id);

                // Delete the user
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    TempData["Success"] = "User and all related data were deleted successfully.";
                }
                else
                {
                    TempData["Error"] = "Failed to delete user account.";
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting user with ID {UserId}", id);
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                TempData["Error"] = "Database error: " + errorMessage;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}