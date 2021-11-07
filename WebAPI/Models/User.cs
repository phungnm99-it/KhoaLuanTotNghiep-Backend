using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class User
    {
        public User()
        {
            Feedbacks = new HashSet<Feedback>();
            OrderShippers = new HashSet<Order>();
            OrderUpdatedByNavigations = new HashSet<Order>();
            OrderUsers = new HashSet<Order>();
            Reviews = new HashSet<Review>();
            StatusUpdateOrders = new HashSet<StatusUpdateOrder>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public bool? IsEmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public int? RoleId { get; set; }
        public bool? IsGoogleLogin { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsDisable { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Order> OrderShippers { get; set; }
        public virtual ICollection<Order> OrderUpdatedByNavigations { get; set; }
        public virtual ICollection<Order> OrderUsers { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<StatusUpdateOrder> StatusUpdateOrders { get; set; }
    }
}
