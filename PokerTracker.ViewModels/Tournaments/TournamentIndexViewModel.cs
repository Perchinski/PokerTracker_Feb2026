using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.ViewModels.Tournaments
{
    /// <summary>
    /// Base view model for tournament cards on the Index page.
    /// Extended by TournamentDetailsViewModel for the full details view.
    /// </summary>
    public class TournamentIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Format { get; set; } = null!;
        
        /// <summary>
        /// Display name of the user who hosts the tournament.
        /// </summary>
        public string Creator { get; set; } = null!;  
        public DateTime Date { get; set; }
        public string Status { get; set; } = null!;
        public string LocationName { get; set; } = null!;
        public string LocationCity { get; set; } = null!;
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Contextual flag denoting if the current viewing user is registered to play in this tournament.
        /// </summary>
        public bool IsJoined { get; set; }
        
        /// <summary>
        /// Contextual flag denoting if the current viewing user has ownership (admin or creator) rights over this tournament.
        /// </summary>
        public bool IsOwner { get; set; }
        
        /// <summary>
        /// Contextual flag denoting if the current viewing user is the definitive creator of this tournament. 
        /// </summary>
        public bool IsCreator { get; set; }
        public string? WinnerName { get; set; }
        public int PlayersCount { get; set; }
    }
}
