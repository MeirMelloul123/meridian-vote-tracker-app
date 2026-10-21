using Meridian.Api.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Meridian.Api.Models
{
    /// <summary>
    /// A difference between approved data and an incoming file, waiting for a human decision.
    /// </summary>
    public class Conflict
    {
        public int Id { get; set; }
        public ConflictType ConflictType { get; set; }

        // What is in conflict
        public int VoteId { get; set; }
        public string? CountryCode { get; set; }          // null for a title conflict

        // The two values: before / proposed by the new file
        public string CurrentValue { get; set; } = null!;   // e.g. "Y"
        public string ProposedValue { get; set; } = null!;  // e.g. "A"
        public int ProposedImportId { get; set; }           // file that proposed the new value
        public DateTimeOffset CreatedAt { get; set; }

        // The human decision (empty while pending)
        public ConflictStatus ConflictStatus { get; set; }
        public int? DecidedByUserId { get; set; }
        public DateTimeOffset? DecidedAt { get; set; }
        public string? Comment { get; set; }

        public Vote Vote { get; set; } = null!;
        [ForeignKey(nameof(CountryCode))]
        public MemberState? MemberState { get; set; }
        public Import ProposedImport { get; set; } = null!;
        [ForeignKey(nameof(DecidedByUserId))]
        public AppUser? DecidedBy { get; set; }
    }
}