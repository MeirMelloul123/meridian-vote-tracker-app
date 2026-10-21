using Meridian.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meridian.Api.Data
{
    public class MeridianContext : DbContext
    {
        public MeridianContext(DbContextOptions<MeridianContext> options) : base(options)
        {
        }
        public DbSet<MemberState> MemberStates { get; set; } = null!;
        public DbSet<MembershipPeriod> MembershipPeriods { get; set; } = null!;
        public DbSet<Vote> Votes { get; set; } = null!;
        public DbSet<CountryVote> CountryVotes { get; set; } = null!;
        public DbSet<Import> Imports { get; set; } = null!;
        public DbSet<AppUser> AppUsers { get; set; } = null!;
        public DbSet<ImportRow> ImportRows { get; set; } = null!;
        public DbSet<Conflict> Conflicts { get; set; } = null!;
        public DbSet<HistoryEvent> HistoryEvents { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Vote: the Assembly code is unique
            modelBuilder.Entity<Vote>()
                .HasIndex(v => v.AssemblyVoteId)
                .IsUnique();

            // CountryVote: one ballot per state per vote
            modelBuilder.Entity<CountryVote>()
                .HasIndex(cv => new { cv.VoteId, cv.CountryCode })
                .IsUnique();
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Store every enum as text ("Y", "Pending"...) instead of a number
            configurationBuilder.Properties<Enum>().HaveConversion<string>();
        }
    }
}
