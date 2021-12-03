using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            StatusUpdateOrders = new HashSet<StatusUpdateOrder>();
        }

        public int Id { get; set; }
        public string OrderCode { get; set; }
        public int? UserId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public bool? IsCompleted { get; set; }
        public decimal TotalCost { get; set; }
        public string PaymentMethod { get; set; }
        public int? ShipperId { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual User Shipper { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<StatusUpdateOrder> StatusUpdateOrders { get; set; }
    }
}
