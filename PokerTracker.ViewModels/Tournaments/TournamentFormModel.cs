using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokerTracker.GCommon.EntityValidation;
using static PokerTracker.GCommon.EntityValidation.Tournament;
using static PokerTracker.GCommon.EntityValidation.Shared;

namespace PokerTracker.ViewModels.Tournaments
{
    /// <summary>
    /// Shared form model for both Add and Edit tournament views.
    /// Formats collection is re-populated by the controller on validation failure.
    /// </summary>
    public class TournamentFormModel : IValidatableObject
    {
        [Required]
        [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
        public string Name { get; set; } = null!;

        [MaxLength(MaxDescriptionLength)]
        public string? Description { get; set; }

        [MaxLength(MaxImageUrlLength)]
        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = ErrorMessages.SelectFormatMessage)]
        [Display(Name = "Format")]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMessages.ValidFormatMessage)]
        public int FormatId { get; set; }

        [Required(ErrorMessage = ErrorMessages.SelectLocationMessage)]
        [Display(Name = "Location")]
        [Range(1, int.MaxValue, ErrorMessage = ErrorMessages.ValidLocationMessage)]
        public int LocationId { get; set; }

        // Not submitted by the form — populated server-side for dropdown rendering
        public IEnumerable<TournamentFormatViewModel> Formats { get; set; }
            = new List<TournamentFormatViewModel>();

        // Populated server-side for dropdown rendering
        public IEnumerable<LocationSelectionViewModel> Locations { get; set; }
            = new List<LocationSelectionViewModel>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Allow setting a date up to 1 day in the past because someone might have forgotten to host/record the tournament on the actual day
            if (Date.Date < DateTime.UtcNow.Date.AddDays(-1))
            {
                yield return new ValidationResult(ErrorMessages.DateInPastMessage, new[] { nameof(Date) });
            }
        }
    }
}
