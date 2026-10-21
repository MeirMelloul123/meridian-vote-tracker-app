using Meridian.Api.Data;
using Meridian.Api.Dto;
using Meridian.Api.Enums;
using Meridian.Api.Models;

namespace Meridian.Api.Service
{
    public class ImportService
    {
        private const string UploadsFolder = @"C:\ProgramData\Meridian\uploads";

        private readonly MeridianContext _context;
        private readonly IServiceScopeFactory _scopeFactory;

        public ImportService(MeridianContext context, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _scopeFactory = scopeFactory;
        }

        public async Task<FileResponse> ImportFile(FileRequestDto fileRequest)
        {
            if (fileRequest.File == null || fileRequest.File.Length == 0)
                return FileResponse.Fail("No file was sent.");

            if (!fileRequest.File.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                return FileResponse.Fail("Only .csv files are accepted.");

            var user = await _context.AppUsers.FindAsync(fileRequest.UserId);
            if (user == null)
                return FileResponse.Fail("Unknown user.");

            // Keep the original file (traceability + background job reads it)
            Directory.CreateDirectory(UploadsFolder);
            var storedPath = Path.Combine(UploadsFolder, $"{Guid.NewGuid()}_{Path.GetFileName(fileRequest.File.FileName)}");

            await using (var stream = File.Create(storedPath))
            {
                await fileRequest.File.CopyToAsync(stream);
            }

            var import = new Import
            {
                FileName = fileRequest.File.FileName,
                StoredFilePath = storedPath,
                UploadedByUserId = fileRequest.UserId,
                UploadedAt = DateTimeOffset.UtcNow,
                Status = ImportStatus.Pending
            };
            _context.Imports.Add(import);

            _context.HistoryEvents.Add(new HistoryEvent
            {
                EventType = HistoryEventType.FileUploaded,
                OccurredAt = DateTimeOffset.UtcNow,
                UserId = fileRequest.UserId,
                Import = import,
                Description = $"{fileRequest.File.FileName} uploaded"
            });

            await _context.SaveChangesAsync();

            // Hand over to the background worker
            var importId = import.Id;
            _ = Task.Run(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<ImportProcessor>();
                await processor.ProcessAsync(importId);
            });

            return FileResponse.Ok(import.Id, "File uploaded, processing started.");
        }
    }
}