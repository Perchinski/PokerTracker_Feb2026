using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Data.Models
{
    using static GCommon.EntityValidation;
    using static GCommon.EntityValidation.Location;
    using static GCommon.EntityValidation.Shared;
    
    /// <summary>
    /// Represents a physical or online venue where a poker tournament can be hosted.
    /// </summary>
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(MaxAddressLength)]
        public string Address { get; set; } = null!;

        [Required]
        [StringLength(MaxCityLength)]
        public string City { get; set; } = null!;

        [Url]
        [StringLength(MaxImageUrlLength)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Determines if new tournaments can be created at this location.
        /// True = active/open for hosting, False = closed/archived.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Global soft delete flag, keeps the location in the db but hides it from queries.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Navigation property: One Location can host many Tournaments.
        /// </summary>
        public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}
