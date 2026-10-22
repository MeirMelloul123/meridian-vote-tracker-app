using Meridian.Api.Data;
using Meridian.Api.Dto;
using Meridian.Api.Enums;
using Meridian.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Meridian.Api.Service
{
    public class ConflictService
    {
        private readonly MeridianContext _db;

        public ConflictService(MeridianContext db)
        {
            _db = db;
        }

        public async Task SaveConflictAsync(int voteId, string? countryCode, string currentValue, string proposedValue, int importId)
        {
            _db.Conflicts.Add(new Models.Conflict
            {
                ConflictType = countryCode == null ? ConflictType.TitleChanged : ConflictType.BallotChanged,
                VoteId = voteId,
                CountryCode = countryCode,
                CurrentValue = currentValue,
                ProposedValue = proposedValue,
                ProposedImportId = importId,
                CreatedAt = DateTimeOffset.UtcNow,
                ConflictStatus = ConflictStatus.Pending
            });
            await _db.SaveChangesAsync();
        }
        public async Task<List<Models.Conflict>> GetPendingAsync()
        {
            return await _db.Conflicts
                .Where(c => c.ConflictStatus == ConflictStatus.Pending)
                .Include(c => c.Vote)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }
        public async Task<bool> DecideAsync(DecisionRequest decisionRequest)
        {
            var status = decisionRequest.IsConflictAccepeted ? ConflictStatus.Accepted : ConflictStatus.Rejected;
            var now = DateTimeOffset.UtcNow;
            var conflict = await _db.Conflicts.FirstOrDefaultAsync(x => x.Id == decisionRequest.Id);    
            if (conflict == null || conflict.ConflictStatus != ConflictStatus.Pending) return false;
            conflict.ConflictStatus = status;
            conflict.DecidedAt = now;
            conflict.Comment = decisionRequest.Comment;
            conflict.DecidedByUserId = decisionRequest.UserId;
            if (decisionRequest.IsConflictAccepeted)
            {
                if (conflict.ConflictType == ConflictType.TitleChanged)
                {
                    var vote = await _db.Votes.FirstAsync(v => v.Id == conflict.VoteId);
                    vote.ResolutionTitle = conflict.ProposedValue;
                }
                else
                {
                    var countryVote = await _db.CountryVotes.FirstAsync(cv =>
                        cv.VoteId == conflict.VoteId && cv.CountryCode == conflict.CountryCode);
                    countryVote.Ballot = Enum.Parse<Ballot>(conflict.ProposedValue);
                    countryVote.SourceImportId = conflict.ProposedImportId;
                }
            }
            _db.HistoryEvents.Add(new HistoryEvent
            {
                EventType = decisionRequest.IsConflictAccepeted ? HistoryEventType.ConflictAccepted : HistoryEventType.ConflictRejected,
                OccurredAt = now,
                UserId = decisionRequest.UserId,
                VoteId = conflict.VoteId,
                CountryCode = conflict.CountryCode,
                OldValue = conflict.CurrentValue,
                NewValue = conflict.ProposedValue,
                ImportId = conflict.ProposedImportId,
                Description = decisionRequest.Comment
            });
           await  _db.SaveChangesAsync();
            return true;
        }
    }
}