using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.ViewModels.Tournaments
{
    /// <summary>
    /// View model used for tracking tournament metadata while a creator/admin chooses the winning participant.
    /// </summary>
    public class SelectWinnerViewModel
    {
        public int TournamentId { get; set; }
        public string? TournamentName { get; set; }

        [Required(ErrorMessage = PokerTracker.GCommon.EntityValidation.ErrorMessages.SelectWinnerMessage)]
        [Display(Name = "Winner")]
        public string WinnerId { get; set; } = null!;

        public IEnumerable<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
    }
}
