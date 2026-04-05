using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.ViewModels.Admin.Users
{
    /// <summary>
    /// Bundles a user's details with a selectable list of roles for authorization management.
    /// </summary>
    public class UserRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<RoleSelectionViewModel> Roles { get; set; } = new List<RoleSelectionViewModel>();
    }

    /// <summary>
    /// Represents a single checkbox toggle for assigning or rescinding a specific role.
    /// </summary>
    public class RoleSelectionViewModel
    {
        public string RoleName { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
