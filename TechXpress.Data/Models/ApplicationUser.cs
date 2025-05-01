using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TechXpress.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add additional properties here
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Navigation property for orders
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ShoppingCart ShoppingCart { get; set; } = new ShoppingCart();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        // Add any other custom user properties you need



    }
}