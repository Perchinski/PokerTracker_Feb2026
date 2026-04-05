using System.ComponentModel.DataAnnotations;
using static PokerTracker.GCommon.EntityValidation;
using static PokerTracker.GCommon.EntityValidation.Location;
using static PokerTracker.GCommon.EntityValidation.Shared;
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
        [StringLength(MaxNameLength, MinimumLength = MinNameLength, ErrorMessage = StringLengthMessage)]
        [Display(Name = "Location Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxAddressLength, MinimumLength = MinAddressLength, ErrorMessage = StringLengthMessage)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(MaxCityLength, MinimumLength = MinCityLength, ErrorMessage = StringLengthMessage)]
        public string City { get; set; } = string.Empty;

        [Url(ErrorMessage = "Invalid URL format.")]
        [StringLength(MaxImageUrlLength, ErrorMessage = StringLengthMessage)]
        public string? ImageUrl { get; set; }

        [Display(Name = "Is Active (Available for new tournaments)")]
        public bool IsActive { get; set; } = true;
    }
}