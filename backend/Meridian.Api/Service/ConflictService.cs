using Meridian.Api.Data;
using Meridian.Api.Enums;
using Meridian.Api.Models;

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
            _db.Conflicts.Add(new Conflict
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
    }
}