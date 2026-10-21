namespace Meridian.Api.Dto
{
    public class FileRequestDto
    {
        public IFormFile File { get; set; } = null!;
        public int UserId { get; set; } = null!;
    }
}
