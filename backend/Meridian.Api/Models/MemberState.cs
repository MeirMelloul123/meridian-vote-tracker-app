namespace Meridian.Api.Models
{
    public class MemberState
    {
        public string? Code { get; set; }   // "BRA", "FRA"...
        public string? Name { get; set; }   // "Brazil", "France"...
        // A state can leave and rejoin, e.g. Iceland (1995-2019, then 2022-present)

        public List<MembershipPeriod> MembershipPeriods { get; set; } = new();
    }
}
