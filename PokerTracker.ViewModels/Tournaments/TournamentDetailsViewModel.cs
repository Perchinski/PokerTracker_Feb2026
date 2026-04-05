using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.ViewModels.Tournaments
{
    /// <summary>
    /// Extended view model for presenting the full details of a tournament, including the location breakdown and participant list.
    /// </summary>
    public class TournamentDetailsViewModel : TournamentIndexViewModel
    {
        public string Description { get; set; } = null!;

        public int LocationId { get; set; }
        public string LocationName { get; set; } = null!;
        public string LocationAddress { get; set; } = string.Empty;
        public string LocationCity { get; set; } = string.Empty;
        public string? LocationImageUrl { get; set; }

        public List<PlayerViewModel> Players { get; set; } = new();
    }
}
