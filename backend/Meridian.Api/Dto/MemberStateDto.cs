namespace Meridian.Api.Dto
{
    public class MemberStateDto
    {
        public string Code { get; set; }= null!;
        public string Name { get; set; }= null!;
        public List<MembershipPeriodDto>? Periods { get; set; }
    }
}
