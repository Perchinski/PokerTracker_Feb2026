using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.ViewModels.Tournaments
{
    public class SelectWinnerViewModel
    {
        public int TournamentId { get; set; }
        public string? TournamentName { get; set; }

        [Required(ErrorMessage = "Please select a winner.")]
        [Display(Name = "Winner")]
        public string WinnerId { get; set; } = null!;

        public IEnumerable<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
    }
}
