using System.ComponentModel.DataAnnotations;

namespace PokerTracker.Data.Models;

using static PokerTracker.GCommon.EntityValidation.Announcement;

public class Announcement
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(MaxTitleLength)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(MaxContentLength)]
    public string Content { get; set; } = null!;

    public DateTime PublishedOn { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Global soft delete flag.
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}
