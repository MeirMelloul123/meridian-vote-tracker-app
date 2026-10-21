using System.ComponentModel.DataAnnotations.Schema;

namespace Meridian.Api.Models
{
    /// <summary>
    /// A period during which a state was a member of the Assembly.
    /// </summary>

    public class MembershipPeriod
    {
        public int Id { get; set; }
        public string CountryCode { get; set; } =null!;
        public DateOnly MemberFrom { get; set; }
        public DateOnly? MemberTo { get; set; }
        [ForeignKey(nameof(CountryCode))]
        public MemberState MemberState { get; set; } =null!;
    }
}
