using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokerTracker.GCommon.EntityValidation;

namespace PokerTracker.Data.Models
{
    public class TournamentFormat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxFormatNameLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Tournament> Tournaments { get; set; } = new HashSet<Tournament>();

    }
}
