using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PokerTracker.Data.Models;

namespace PokerTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tournament> Tournaments { get; set; } = null!;
        public virtual DbSet<TournamentFormat> TournamentFormats { get; set; } = null!;
        public virtual DbSet<PlayerTournament> PlayersTournaments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PlayerTournament>()
                .HasKey(pt => new { pt.PlayerId, pt.TournamentId });


            builder.Entity<PlayerTournament>()
                .HasOne(pt => pt.Tournament)
                .WithMany(t => t.PlayersTournaments)
                .HasForeignKey(pt => pt.TournamentId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to keep history

            builder.Entity<PlayerTournament>()
                .HasOne(pt => pt.Player)
                .WithMany() 
                .HasForeignKey(pt => pt.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Tournament>()
                .HasOne(t => t.Creator)
                .WithMany()
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.Restrict); // Don't delete tournament if Creator is deleted

            builder.Entity<Tournament>()
                .HasOne(t => t.Winner)
                .WithMany()
                .HasForeignKey(t => t.WinnerId)
                .OnDelete(DeleteBehavior.SetNull); // If Winner is deleted, keep tournament but set Winner to NULL

            builder.Entity<Tournament>()
                .HasOne(t => t.Format)
                .WithMany(f => f.Tournaments)
                .HasForeignKey(t => t.FormatId)
                .OnDelete(DeleteBehavior.Restrict); // Cannot delete a Format if it's used by tournaments

            // Seed initial tournament formats
            builder.Entity<TournamentFormat>().HasData(
                new TournamentFormat { Id = 1, Name = "Texas Hold'em - No Limit" },
                new TournamentFormat { Id = 2, Name = "Texas Hold'em - Fixed Limit" },
                new TournamentFormat { Id = 3, Name = "Omaha Pot Limit" },
                new TournamentFormat { Id = 4, Name = "Sit & Go" },
                new TournamentFormat { Id = 5, Name = "Spin & Go" },
                new TournamentFormat { Id = 6, Name = "Bounty / Knockout" }
            );
        }
    }
}

