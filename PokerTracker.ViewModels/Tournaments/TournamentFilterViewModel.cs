using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.ViewModels.Tournaments
{
    public class TournamentFilterViewModel
    {
        public List<TournamentIndexViewModel> Tournaments { get; set; } = new();

        public string? SearchTerm { get; set; }
        public int? FormatId { get; set; }
        public string Status { get; set; } = null!;
        public string SortOrder { get; set; } = "date_asc"; 

        public bool OnlyJoined { get; set; }
        public IEnumerable<TournamentFormatViewModel> Formats { get; set; } = new List<TournamentFormatViewModel>();


    }
}
