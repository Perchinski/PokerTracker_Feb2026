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
    /// <summary>
    /// Seeds player registrations — both users joined both tournaments.
    /// </summary>
    public class PlayerTournamentSeedConfiguration : IEntityTypeConfiguration<PlayerTournament>
    {
        public void Configure(EntityTypeBuilder<PlayerTournament> builder)
        {
            builder.HasData(
                // Both players joined the Open tournament
                new PlayerTournament { PlayerId = SeedConstants.PlayerOneId, TournamentId = 1 },
                new PlayerTournament { PlayerId = SeedConstants.PlayerTwoId, TournamentId = 1 },

                // Both players were in the Finished tournament
                new PlayerTournament { PlayerId = SeedConstants.PlayerOneId, TournamentId = 2 },
                new PlayerTournament { PlayerId = SeedConstants.PlayerTwoId, TournamentId = 2 }
            );
        }
    }
}

