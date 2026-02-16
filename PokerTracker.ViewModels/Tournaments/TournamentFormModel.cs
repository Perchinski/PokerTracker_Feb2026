using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokerTracker.GCommon.EntityValidation;

namespace PokerTracker.ViewModels.Tournaments
{
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

        public IEnumerable<TournamentFormatViewModel> Formats { get; set; }
            = new List<TournamentFormatViewModel>();
    }
}
