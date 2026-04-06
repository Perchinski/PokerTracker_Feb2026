using System.ComponentModel.DataAnnotations;

namespace PokerTracker.ViewModels.Announcements;

using static PokerTracker.GCommon.EntityValidation.Announcement;

public class AnnouncementViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(MaxTitleLength, MinimumLength = MinTitleLength)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(MaxContentLength, MinimumLength = MinContentLength)]
    public string Content { get; set; } = null!;

    public DateTime PublishedOn { get; set; }

    public bool IsActive { get; set; }
}
