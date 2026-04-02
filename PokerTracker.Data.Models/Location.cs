using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Data.Models
{
    using static GCommon.EntityValidation;
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxLocationNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(MaxLocationAddressLength)]
        public string Address { get; set; } = null!;

        [Required]
        [StringLength(MaxLocationCityLength)]
        public string City { get; set; } = null!;

        // true = open for new tournaments, false = closed/archived
        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        // Navigation property: One Location can host MANY Tournaments
        public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}
