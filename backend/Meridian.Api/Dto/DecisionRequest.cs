namespace Meridian.Api.Dto
{
    public class DecisionRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsConflictAccepeted { get; set; }
        public string? Comment { get; set; }
    }
}
