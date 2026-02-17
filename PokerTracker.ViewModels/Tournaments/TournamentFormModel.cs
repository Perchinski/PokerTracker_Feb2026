using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokerTracker.GCommon.EntityValidation;

namespace PokerTracker.ViewModels.Tournaments
{
    /// <summary>
    /// Shared form model for both Add and Edit tournament views.
    /// Formats collection is re-populated by the controller on validation failure.
    /// </summary>
    public class TournamentFormModel
    {
        [Required]
        [StringLength(MaxTournamentNameLength, MinimumLength = MinTournamentNameLength)]
        public string Name { get; set; } = null!;

        [MaxLength(MaxTournamentDescriptionLength)]
        public string? Description { get; set; }

        [MaxLength(MaxImageUrlLength)]
        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please select a format.")]
        [Display(Name = "Format")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid format.")]
        public int FormatId { get; set; }

        // Not submitted by the form — populated server-side for dropdown rendering
        public IEnumerable<TournamentFormatViewModel> Formats { get; set; }
            = new List<TournamentFormatViewModel>();
    }
}
