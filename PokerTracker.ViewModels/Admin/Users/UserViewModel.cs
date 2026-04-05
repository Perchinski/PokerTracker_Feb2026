namespace PokerTracker.ViewModels.Admin.Users
{
    /// <summary>
    /// Serves as the primary display model for registered Users in the Admin area list.
    /// </summary>
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}