using System.ComponentModel.DataAnnotations;
using static PokerTracker.GCommon.EntityValidation;
using static PokerTracker.GCommon.EntityValidation.ErrorMessages;

namespace PokerTracker.ViewModels.Admin.Locations
{
    // Used for listing locations in the table
    public class LocationViewModels
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int TournamentCount { get; set; }
    }

    // Used for the Create and Edit forms
    public class LocationFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxLocationNameLength, MinimumLength = MinLocationNameLength, ErrorMessage = StringLengthMessage)]
        [Display(Name = "Location Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxLocationAddressLength, MinimumLength = MinLocationAddressLength, ErrorMessage = StringLengthMessage)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxLocationCityLength, MinimumLength = MinLocationCityLength, ErrorMessage = StringLengthMessage)]
        public string City { get; set; } = string.Empty;

        [Url(ErrorMessage = "Invalid URL format.")]
        [StringLength(MaxImageUrlLength, ErrorMessage = StringLengthMessage)]
        public string? ImageUrl { get; set; }

        [Display(Name = "Is Active (Available for new tournaments)")]
        public bool IsActive { get; set; } = true;
    }
}