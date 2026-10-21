using System.ComponentModel.DataAnnotations;

namespace Meridian.Api.Models
{
    /// <summary>
    /// A member state of the Assembly, identified by its ISO code.
    /// </summary>
    public class MemberState
    {
        [Key]
        public string Code { get; set; } = null!;   // ISO 3166 alpha-3, e.g. "BRA"
        public string Name { get; set; } = null!;   // e.g. "Brazil"

        // A state can leave and rejoin, e.g. Iceland (1995-2019, then 2022-present)
        public List<MembershipPeriod> MembershipPeriods { get; set; } = new();
    }
}