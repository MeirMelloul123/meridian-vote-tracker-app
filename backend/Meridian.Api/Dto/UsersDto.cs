namespace Meridian.Api.Dto
{
    public class UsersDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }   // "Dana", "Omar", "Lena"
        public string Role { get; set; }=null!;  // "Admin", "User", "Guest" 
    }
}
