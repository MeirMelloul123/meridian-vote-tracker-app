using Meridian.Api.Data;
using Meridian.Api.Enums;
using Meridian.Api.Models;
using Meridian.Api.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Meridian.Api.Service
{
    /// <summary>
    /// Reads an uploaded file in the background and processes its rows.
    /// </summary>
    public class ImportProcessor
    {
        // Only one import at a time
        private static readonly SemaphoreSlim _lock = new(1, 1);

        private readonly MeridianContext _db;
        private readonly VoteService _voteService;
        private readonly ConflictService _conflictService;

        public ImportProcessor(MeridianContext db, VoteService voteService, ConflictService conflictService)
        {
            _db = db;
            _voteService = voteService;
            _conflictService = conflictService;
        }

        public async Task ProcessAsync(int importId)
        {
            await _lock.WaitAsync();
            try
            {
                var import = await _db.Imports.FindAsync(importId);
                if (import == null) return;

                var lines = await File.ReadAllLinesAsync(import.StoredFilePath);
                import.Status = ImportStatus.Processing;
                import.TotalRows = lines.Length - 1;   // minus the header
                await _db.SaveChangesAsync();

                var countryCodes = await _db.MemberStates.Select(m => m.Code).ToListAsync();

                for (int i = 1; i < lines.Length; i++)
                {
                    var columns = CsvUtils.SplitCsvLine(lines[i]);
                    // 0 vote id, 1 session, 2 date, 3 title, 4 country code, 5 country name, 6 ballot

                    var row = new ImportRow
                    {
                        ImportId = import.Id,
                        RowNumber = i + 1,   // line number in the file
                        AssemblyVoteId = columns[0],
                        CountryCode = columns[4],
                        Ballot = columns[6],
                        Status = RowStatus.New
                    };

                    var error = CsvUtils.CheckRow(columns, countryCodes);
                    if (error != null)
                    {
                        row.Status = RowStatus.Invalid;
                        row.Reason = error;
                        _db.ImportRows.Add(row);
                        import.ProcessedRows++;
                        continue;
                    }

                    var vote = await _voteService.FindOrCreateVoteAsync(columns[0], columns, import.Id);
                    row.Status = await _voteService.SaveCountryVoteAsync(vote, columns[4], Enum.Parse<Ballot>(columns[6]), import.Id);

                    if (row.Status == RowStatus.Conflict)
                    {
                        var current = await _db.CountryVotes
                            .Where(cv => cv.VoteId == vote.Id && cv.CountryCode == columns[4])
                            .Select(cv => cv.Ballot)
                            .FirstAsync();
                        row.Reason = $"Approved value is {current}, file says {columns[6]}";
                        await _conflictService.SaveConflictAsync(vote.Id, columns[4], current.ToString(), columns[6], import.Id);
                    }

                    _db.ImportRows.Add(row);

                    import.ProcessedRows++;
                    if (import.ProcessedRows % 500 == 0)
                        await _db.SaveChangesAsync();   // progress bar
                }

                import.Status = ImportStatus.Completed;
                await _db.SaveChangesAsync();
            }
            finally
            {
                _lock.Release();
            }
        }
        
    }
}