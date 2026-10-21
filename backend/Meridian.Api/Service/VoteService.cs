using Meridian.Api.Data;
using Meridian.Api.Enums;
using Meridian.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Meridian.Api.Service
{
    public class VoteService
    {
        private readonly MeridianContext _db;
        private readonly ConflictService _conflictService;

        public VoteService(MeridianContext db, ConflictService conflictService)
        {
            _db = db;
            _conflictService = conflictService;
        }

        public async Task<Vote> FindOrCreateVoteAsync(string assemblyVoteId, List<string> line, int importId)
        {
            var vote = await _db.Votes.FirstOrDefaultAsync(v =>
                v.AssemblyVoteId == assemblyVoteId);

            if (vote == null)
            {
                vote = new Vote
                {
                    AssemblyVoteId = assemblyVoteId,
                    Session = int.Parse(line[1]),
                    VoteDate = DateOnly.ParseExact(line[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    ResolutionTitle = line[3]
                    // DesiredOutcome and Weight stay empty: set later by an admin
                };
                _db.Votes.Add(vote);
                await _db.SaveChangesAsync();
            }
            else if (vote.ResolutionTitle != line[3])
            {
                await _conflictService.SaveConflictAsync(vote.Id, null, vote.ResolutionTitle, line[3], importId);
            }

            return vote;
        }

        public async Task<RowStatus> SaveCountryVoteAsync(Vote vote, string countryCode, Ballot ballot, int importId)
        {
            var existing = await _db.CountryVotes.FirstOrDefaultAsync(cv =>
                cv.VoteId == vote.Id && cv.CountryCode == countryCode);

            if (existing == null)
            {
                // No approved value yet: store it
                _db.CountryVotes.Add(new CountryVote
                {
                    VoteId = vote.Id,
                    CountryCode = countryCode,
                    Ballot = ballot,
                    SourceImportId = importId
                });
                return RowStatus.New;
            }

            if (existing.Ballot == ballot)
                return RowStatus.Unchanged;

            return RowStatus.Conflict;
        }
    }
}