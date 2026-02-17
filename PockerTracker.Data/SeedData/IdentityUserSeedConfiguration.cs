using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Data.SeedData
{
    /// <summary>
    /// Seeds two demo users with pre-hashed passwords.
    /// Login credentials:
    ///   - player1@pokertracker.com / Player1!
    ///   - player2@pokertracker.com / Player2!
    /// </summary>
    public class IdentityUserSeedConfiguration : IEntityTypeConfiguration<IdentityUser>
    {
        public void Configure(EntityTypeBuilder<IdentityUser> builder)
        {
            var hasher = new PasswordHasher<IdentityUser>();

            var playerOne = new IdentityUser
            {
                Id = SeedConstants.PlayerOneId,
                UserName = "player1@pokertracker.com",
                NormalizedUserName = "PLAYER1@POKERTRACKER.COM",
                Email = "player1@pokertracker.com",
                NormalizedEmail = "PLAYER1@POKERTRACKER.COM",
                EmailConfirmed = true,
                SecurityStamp = "STATIC-SECURITY-STAMP-P1"
            };
            playerOne.PasswordHash = hasher.HashPassword(playerOne, "Player1!");

            var playerTwo = new IdentityUser
            {
                Id = SeedConstants.PlayerTwoId,
                UserName = "player2@pokertracker.com",
                NormalizedUserName = "PLAYER2@POKERTRACKER.COM",
                Email = "player2@pokertracker.com",
                NormalizedEmail = "PLAYER2@POKERTRACKER.COM",
                EmailConfirmed = true,
                SecurityStamp = "STATIC-SECURITY-STAMP-P2"
            };
            playerTwo.PasswordHash = hasher.HashPassword(playerTwo, "Player2!");

            builder.HasData(playerOne, playerTwo);
        }
    }

}
