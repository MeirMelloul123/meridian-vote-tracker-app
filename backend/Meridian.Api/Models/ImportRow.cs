using Meridian.Api.Enums;

namespace Meridian.Api.Models
{
    /// <summary>
    /// The outcome of one row of an imported file (the import report).
    /// </summary>
    public class ImportRow
    {
        public long Id { get; set; }
        public int ImportId { get; set; }          // which file
        public int RowNumber { get; set; }         // line number in the file

        // Original values, exactly as written in the file
        public string AssemblyVoteId { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string Ballot { get; set; } = null!;

        // What we did with this row, and why
        public RowStatus Status { get; set; }
        public string? Reason { get; set; }

        public Import Import { get; set; } = null!; 
    }
}