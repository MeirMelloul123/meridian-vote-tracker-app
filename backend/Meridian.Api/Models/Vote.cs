using Meridian.Api.Enums;

namespace Meridian.Api.Models
{
    /// <summary>
    /// A single vote of the Assembly (one resolution).
    /// </summary>
    public class Vote
    {
        public int Id { get; set; }
        public string AssemblyVoteId { get; set; } = null!;    // e.g. "MA/2024/001", unique
        public int Session { get; set; }
        public DateOnly VoteDate { get; set; }
        public string ResolutionTitle { get; set; } = null!;
        // Northwind position (set by an admin)
        public Ballot? DesiredOutcome { get; set; }   // Y or N, null = no position yet
        public int? Weight { get; set; }              // 1 (minor) to 5 (critical)
        public List<CountryVote> CountryVotes { get; set; } = new();
    }
}
