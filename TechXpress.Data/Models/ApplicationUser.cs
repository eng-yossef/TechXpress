using Microsoft.AspNetCore.Identity;

namespace TechXpress.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add additional properties here
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Add any other custom user properties you need
    }
}