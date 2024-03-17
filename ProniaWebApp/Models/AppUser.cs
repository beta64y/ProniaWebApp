using Microsoft.AspNetCore.Identity;

namespace ProniaWebApp.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
        public bool IsActive { get; set; }
    }
}
