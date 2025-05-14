using System;
using System.Collections.Generic;

namespace backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; } // User, Staff, Admin, Member
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int SuccessfulOrders { get; set; } = 0; // For tracking discount eligibility
        public bool HasStackableDiscount { get; set; } = false; // 10% discount after 10 successful orders
        
        // Navigation properties
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
