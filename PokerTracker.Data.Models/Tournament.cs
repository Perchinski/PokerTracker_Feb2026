using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokerTracker.GCommon.EntityValidation;

namespace PokerTracker.Data.Models
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxTournamentNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        public int FormatId { get; set; }

        [ForeignKey(nameof(FormatId))]
        public virtual TournamentFormat Format { get; set; } = null!;

        [MaxLength(MaxTournamentDescriptionLength)]
        public string? Description { get; set; }

        [Url]
        [MaxLength(MaxImageUrlLength)]
        public string? ImageUrl { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string CreatorId { get; set; } = null!;

        [ForeignKey(nameof(CreatorId))]
        public virtual IdentityUser Creator { get; set; } = null!;

        public string? WinnerId { get; set; }

        // Nullable — set after tournament finishes via SelectWinner
        [ForeignKey(nameof(WinnerId))]
        public virtual IdentityUser? Winner { get; set; }

        public TournamentStatus Status { get; set; }

        // Soft delete flag — filtered globally via HasQueryFilter in DbContext
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<PlayerTournament> PlayersTournaments { get; set; }
        = new HashSet<PlayerTournament>();


    }
}
