using Meridian.Api.Enums;

namespace Meridian.Api.Models
{
    /// <summary>
    /// The approved ballot of one state in one vote.
    /// </summary>
    public class CountryVote
    {
        public int Id { get; set; }
        public int VoteId { get; set; }
        public string? CountryCode { get; set; }
        public Ballot Ballot { get; set; }

        public int SourceImportId { get; set; }   // file that brought the approved value

        public Vote Vote { get; set; } = null!;
        public MemberState MemberState { get; set; } = null!;
        public Import SourceImport { get; set; } = null!;
    }
}