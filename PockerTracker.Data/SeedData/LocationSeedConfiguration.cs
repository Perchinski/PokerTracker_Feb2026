using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerTracker.Data.Models;

namespace PokerTracker.Data.SeedData
{
    public class LocationSeedConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasData(
                new Location
                {
                    Id = 1,
                    Name = "Bellagio Casino",
                    Address = "3600 S Las Vegas Blvd",
                    City = "Las Vegas",
                    ImageUrl = "https://images.unsplash.com/photo-1590059530490-25251624c478?w=800",
                    IsActive = true,
                    IsDeleted = false
                },
                new Location
                {
                    Id = 2,
                    Name = "Downtown Poker Club",
                    Address = "123 Main Street",
                    City = "New York",
                    ImageUrl = "https://images.unsplash.com/photo-1596484552834-6a58f850e0a1?w=800",
                    IsActive = true,
                    IsDeleted = false
                }
            );
        }
    }
}