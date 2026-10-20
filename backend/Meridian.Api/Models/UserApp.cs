using Meridian.Api.Enums;

namespace Meridian.Api.Models
{
    /// <summary>
    /// A user of the system 
    /// </summary>
    public class AppUser
    {
        public int Id { get; set; }
        public string? Name { get; set; }   // "Dana", "Omar", "Lena"
        public UserRole Role { get; set; }
    }
}