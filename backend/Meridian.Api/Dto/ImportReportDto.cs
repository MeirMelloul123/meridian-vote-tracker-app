namespace Meridian.Api.Dto
{
    public class ImportReportDto
    {
        public int ImportId { get; set; }
        public string FileName { get; set; } = null!;
        public string UploadedBy { get; set; } = null!;
        public DateTimeOffset UploadedAt { get; set; }
        public string Status { get; set; } = null!;
        public int TotalRows { get; set; }
        // Counters: "New" -> 5880, "Conflict" -> 2...          ⬅️ morceau 2
        public Dictionary<string, int> Counts { get; set; } = new();
        // Rows that were not stored
        public List<ImportRowDto> RowsNotStored { get; set; } = new();
    }
}