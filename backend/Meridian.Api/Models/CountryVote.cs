using Meridian.Api.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Meridian.Api.Models
{
    /// <summary>
    /// The approved ballot of one state in one vote.
    /// </summary>
    public class CountryVote
    {
        public int Id { get; set; }
        public int VoteId { get; set; }
        public string CountryCode { get; set; } = null!;
        public Ballot Ballot { get; set; }

        public int SourceImportId { get; set; }   // file that brought the approved value
    
        public Vote Vote { get; set; } = null!;
        [ForeignKey(nameof(CountryCode))]
        public MemberState MemberState { get; set; } = null!;
        [ForeignKey(nameof(SourceImportId))]
        public Import SourceImport { get; set; } = null!;
    }
}