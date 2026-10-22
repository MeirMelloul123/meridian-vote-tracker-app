namespace Meridian.Api.Dto
{
    public class ImportRowDto
    {
        public int RowNumber { get; set; }
        public string? AssemblyVoteId { get; set; }
        public string? CountryCode { get; set; }
        public string? Ballot { get; set; }
        public string Status { get; set; } = null!;
        public string? Reason { get; set; }
    }
}
