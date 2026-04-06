using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerTracker.Data.Models;

namespace PokerTracker.Data.SeedData;

public class AnnouncementSeedConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.HasData(
            new Announcement
            {
                Id = 1,
                Title = "Welcome to the New Poker Tracker!",
                Content = "We are thrilled to launch the new version of our internal poker tracking system. You can now easily create tournaments, manage players, and keep track of your local rankings.\n\nMore features like leaderboards and advanced statistics will be coming soon!",
                PublishedOn = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
                IsActive = true,
                IsDeleted = false
            }
        );
    }
}