using System.Globalization;
using Meridian.Api.Enums;
using Meridian.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meridian.Api.Data
{
    /// <summary>
    /// Loads reference data (users and member states) when the database is empty.
    /// </summary>
    public  class DbSeed
    {
        private readonly MeridianContext _db;

        public DbSeed(MeridianContext db)
        {
            _db = db;
        }

        public  async Task SeedAsync()
        {
            await SeedUsersAsync();
            await SeedMemberStatesAsync();
        }

        private  async Task SeedUsersAsync()
        {
            // Already seeded: do nothing
            if (await _db.AppUsers.AnyAsync()) return;

            _db.AppUsers.AddRange(
                new AppUser { Name = "Dana", Role = UserRole.Analyst },
                new AppUser { Name = "Omar", Role = UserRole.Analyst },
                new AppUser { Name = "Lena", Role = UserRole.Admin });

            await _db.SaveChangesAsync();
        }

        private  async Task SeedMemberStatesAsync()
        {
            // Already seeded: do nothing
            if (await _db.MemberStates.AnyAsync()) return;

            var path = Path.Combine(AppContext.BaseDirectory, "Seed", "member_states.csv");
            var lines = await File.ReadAllLinesAsync(path);

            // One MemberState per code
            var states = new Dictionary<string, MemberState>();

            foreach (var line in lines.Skip(1))   // skip the header line
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var cols = line.Split(',');
                var code = cols[0].Trim();

                if (!states.TryGetValue(code, out var state))
                {
                    state = new MemberState { Code = code, Name = cols[1].Trim() };
                    states.Add(code, state);
                }

                state.MembershipPeriods.Add(new MembershipPeriod
                {
                    MemberFrom = ParseDate(cols[2])!.Value,
                    MemberTo = ParseDate(cols[3])     // empty = still a member
                });
            }

            _db.MemberStates.AddRange(states.Values);
            await _db.SaveChangesAsync();
        }

        private static DateOnly? ParseDate(string value) =>
            string.IsNullOrWhiteSpace(value)
                ? null
                : DateOnly.ParseExact(value.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
}