using Meridian.Api.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Meridian.Api.Models
{
    /// <summary>
    /// One entry of the history log. Rows are only added, never changed or deleted.
    /// </summary>
    public class HistoryEvent
    {
        public long Id { get; set; }
        public HistoryEventType EventType { get; set; }
        public DateTimeOffset OccurredAt { get; set; }

        // Who did it (null = the system)
        public int? UserId { get; set; }

        // What it is about
        public int? VoteId { get; set; }
        public string? CountryCode { get; set; }

        // What changed (ballot, title, position...)
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        // Source file, if any
        public int? ImportId { get; set; }

        // Short text shown in the timeline
        public string? Description { get; set; }
        [ForeignKey(nameof(CountryCode))]
        public MemberState? MemberState { get; set; }
        public AppUser? User { get; set; }
        public Vote? Vote { get; set; }
        public Import? Import { get; set; }
    }
}