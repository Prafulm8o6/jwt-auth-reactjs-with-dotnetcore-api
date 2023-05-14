using Microsoft.AspNetCore.Identity;

namespace JwtAuthCore.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImg { get; set; }
    }
}
