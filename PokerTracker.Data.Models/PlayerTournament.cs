using Microsoft.AspNetCore.Identity;

namespace PokerTracker.Data.Models
{
    public class PlayerTournament
    {
        public string PlayerId { get; set; }
        public virtual IdentityUser Player { get; set; } = null!;

        public int TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; } = null!;
    }
}