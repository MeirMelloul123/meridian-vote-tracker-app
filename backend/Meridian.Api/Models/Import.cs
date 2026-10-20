using Meridian.Api.Enums;

namespace Meridian.Api.Models
{
    /// <summary>
    /// One uploaded CSV file and the progress of its processing.
    /// </summary>
    public class Import
    {
        // info about the uploaded file
        public int Id { get; set; }
        public string? FileName { get; set; } 
        public int UploadedByUserId { get; set; }
        public DateTimeOffset UploadedAt { get; set; }

        // path to the file on disk (where is read by the background job)
        public string? StoredFilePath { get; set; }

        // Progress (survives a page refresh)
        public ImportStatus Status { get; set; }
        public int TotalRows { get; set; }
        public int ProcessedRows { get; set; }

        public AppUser UploadedBy { get; set; } = null!;
    }
}