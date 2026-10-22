using Meridian.Api.Data;
using Meridian.Api.Dto;
using Meridian.Api.Enums;
using Meridian.Api.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<ImportDto>> GetImports()
        {
            return await _context.Imports
                .OrderByDescending(i => i.UploadedAt)
                .Select(i => new ImportDto
                {
                    Id = i.Id,
                    FileName = i.FileName,
                    UploadedBy = i.UploadedBy.Name,
                    UploadedAt = i.UploadedAt,
                    Status = i.Status.ToString(),
                    TotalRows = i.TotalRows,
                    ProcessedRows = i.ProcessedRows
                })
                .ToListAsync();
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
        public async Task<ImportReportDto?> GetReport(int id)
        {
            Import? import = await _context.Imports.Include(x=>x.UploadedBy).FirstOrDefaultAsync(x => x.Id == id);
            if (import == null) return null;
            ImportReportDto importReportDto = new ImportReportDto();
            importReportDto.ImportId = id;
            importReportDto.FileName = import.FileName;
            importReportDto.UploadedBy = import.UploadedBy?.Name ?? string.Empty;
            importReportDto.Status = import.Status.ToString();
            importReportDto.UploadedAt = import.UploadedAt;
            importReportDto.TotalRows = import.TotalRows;
            importReportDto.Counts= await _context.ImportRows.Where(y=>y.ImportId==id).GroupBy(x=>x.Status).Select(g => new { Status = g.Key, Count = g.Count() }).ToDictionaryAsync(g => g.Status.ToString(), g => g.Count);
            importReportDto.RowsNotStored = await _context.ImportRows .Where(y => y.ImportId == id && y.Status != RowStatus.New && y.Status != RowStatus.Unchanged).OrderBy(y => y.RowNumber)
                                                                        .Select(y => new ImportRowDto
                                                                        {
                                                                            RowNumber = y.RowNumber,
                                                                            AssemblyVoteId = y.AssemblyVoteId,
                                                                            CountryCode = y.CountryCode,
                                                                            Ballot = y.Ballot,
                                                                            Status = y.Status.ToString(),
                                                                            Reason = y.Reason
                                                                        }).ToListAsync();
            return importReportDto;

        }
    }

}