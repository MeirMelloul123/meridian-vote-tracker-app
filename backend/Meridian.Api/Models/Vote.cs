namespace Meridian.Api.Models
{
    /// <summary>
    /// A single vote of the Assembly (one resolution).
    /// </summary>
    public class Vote
    {
        public int Id { get; set; }
        public string? AssemblyVoteId { get; set; }    // e.g. "MA/2024/001", unique
        public int Session { get; set; }
        public DateOnly VoteDate { get; set; }
        public string ResolutionTitle { get; set; } = null!;

        public List<CountryVote> CountryVotes { get; set; } = new();
    }
}
