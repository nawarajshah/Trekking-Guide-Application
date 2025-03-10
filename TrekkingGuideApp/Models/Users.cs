using Microsoft.AspNetCore.Identity;

namespace TrekkingGuideApp.Models
{
    public class Users : IdentityUser
    {
        // Basic fileds that everyone cal fill
        public required string FullName { get; set; }

        // additional fields only for certain roles
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
