using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.ViewModels.Tournaments
{
    public class TournamentDetailsViewModel : TournamentIndexViewModel
    {
        public string Description { get; set; } = null!;

        public List<PlayerViewModel> Players { get; set; } = new();
    }
}
