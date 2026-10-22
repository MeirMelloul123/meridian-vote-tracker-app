namespace Meridian.Api.Dto
{
    public class ImportDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string UploadedBy { get; set; } = null!;
        public DateTimeOffset UploadedAt { get; set; }
        public string Status { get; set; } = null!;
        public int TotalRows { get; set; }
        public int ProcessedRows { get; set; }
    }
}