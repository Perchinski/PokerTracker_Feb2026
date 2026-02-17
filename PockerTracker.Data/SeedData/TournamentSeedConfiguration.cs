using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokerTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Data.SeedData
{
    public class TournamentSeedConfiguration : IEntityTypeConfiguration<Tournament>
    {
        public void Configure(EntityTypeBuilder<Tournament> builder)
        {
            builder.HasData(
                // Open tournament created by Player 1
                new Tournament
                {
                    Id = 1,
                    Name = "Friday Night Holdem",
                    Description = "Casual no-limit game. $20 buy-in, winner takes all!",
                    FormatId = 1, // Texas Hold'em - No Limit
                    Date = new DateTime(2026, 3, 14, 20, 0, 0),
                    CreatorId = SeedConstants.PlayerOneId,
                    Status = TournamentStatus.Open,
                    ImageUrl = "https://images.unsplash.com/photo-1511193311914-0346f16efe90?w=800"
                },
                // Finished tournament created by Player 2, won by Player 1
                new Tournament
                {
                    Id = 2,
                    Name = "Weekend Bounty Bash",
                    Description = "Knockout format — collect a bounty for every player you eliminate.",
                    FormatId = 6, // Bounty / Knockout
                    Date = new DateTime(2026, 2, 8, 19, 0, 0),
                    CreatorId = SeedConstants.PlayerTwoId,
                    WinnerId = SeedConstants.PlayerOneId,
                    Status = TournamentStatus.Finished,
                    ImageUrl = "https://images.unsplash.com/photo-1609902726285-00668009f004?w=800"
                }
            );
        }
    }
}
