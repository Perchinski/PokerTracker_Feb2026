using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.ViewModels.Tournaments
{
    /// <summary>
    /// Wraps the tournament list together with filter/sort state
    /// so the Index view can preserve selections after a round-trip.
    /// </summary>
    public class TournamentFilterViewModel
    {
        public List<TournamentIndexViewModel> Tournaments { get; set; } = new();

        public string? SearchTerm { get; set; }
        public int? FormatId { get; set; }
        public string Status { get; set; } = null!;
        public string SortOrder { get; set; } = "date_asc"; 

        public bool OnlyJoined { get; set; }
        public bool OnlyOwned { get; set; }
        public IEnumerable<TournamentFormatViewModel> Formats { get; set; } = new List<TournamentFormatViewModel>();


    }
}
