namespace Meridian.Api.Models
{
    /// <summary>
    /// A period during which a state was a member of the Assembly.
    /// </summary>

    public class MembershipPeriod
    {
        public int Id { get; set; }
        public string? CountryCode { get; set; }
        public DateOnly MemberFrom { get; set; }
        public DateOnly? MemberTo { get; set; }    

        public MemberState? MemberState { get; set; } 
    }
}
