using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokerTracker.GCommon.EntityValidation;
using static PokerTracker.GCommon.EntityValidation.Tournament;
using static PokerTracker.GCommon.EntityValidation.Shared;

namespace PokerTracker.Data.Models
{
    /// <summary>
    /// Represents the primary domain entity for a Poker Tournament.
    /// Tracks all details including location, format, timeline, participants, and status.
    /// </summary>
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        public int FormatId { get; set; }

        [ForeignKey(nameof(FormatId))]
        public virtual TournamentFormat Format { get; set; } = null!;

        [MaxLength(MaxDescriptionLength)]
        public string? Description { get; set; }

        [Url]
        [MaxLength(MaxImageUrlLength)]
        public string? ImageUrl { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string CreatorId { get; set; } = null!;

        /// <summary>
        /// Navigation property to the User who created (hosts) the tournament.
        /// </summary>
        [ForeignKey(nameof(CreatorId))]
        public virtual IdentityUser Creator { get; set; } = null!;

        public string? WinnerId { get; set; }

        /// <summary>
        /// Navigation property to the User who won the tournament. 
        /// Nullable — only set after the tournament finishes via SelectWinner action.
        /// </summary>
        [ForeignKey(nameof(WinnerId))]
        public virtual IdentityUser? Winner { get; set; }

        /// <summary>
        /// Represents the current lifecycle state of the tournament (e.g., Open, Running, Finished).
        /// </summary>
        public TournamentStatus Status { get; set; }

        /// <summary>
        /// Soft delete flag. When true, the tournament is hidden globally via EF Core HasQueryFilter.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Navigation property to the participants joining this tournament.
        /// </summary>
        public virtual ICollection<PlayerTournament> PlayersTournaments { get; set; }
        = new HashSet<PlayerTournament>();

        [Required]
        public int LocationId { get; set; }

        /// <summary>
        /// Navigation property resolving the physical or virtual location of the tournament.
        /// </summary>
        [ForeignKey(nameof(LocationId))]
        public Location Location { get; set; } = null!;

    }
}
