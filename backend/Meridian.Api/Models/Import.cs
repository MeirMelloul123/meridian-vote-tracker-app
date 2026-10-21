using Meridian.Api.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Meridian.Api.Models
{
    /// <summary>
    /// One uploaded CSV file and the progress of its processing.
    /// </summary>
    public class Import
    {
        // info about the uploaded file
        public int Id { get; set; }
        public string FileName { get; set; } = null!; 
        public int UploadedByUserId { get; set; }
        public DateTimeOffset UploadedAt { get; set; }

        // path to the file on disk (where is read by the background job)
        public string StoredFilePath { get; set; } = null!;

        // Progress (survives a page refresh)
        public ImportStatus Status { get; set; }
        // number of rows in the file
        public int TotalRows { get; set; }
        //number of rows that have been processed (successfully or not)
        public int ProcessedRows { get; set; }
        
        [ForeignKey(nameof(UploadedByUserId))]
        public AppUser UploadedBy { get; set; } = null!;
    }
}