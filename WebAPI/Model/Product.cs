using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Model
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int? Stock { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public int? BrandId { get; set; }
        public string Cpu { get; set; }
        public string Os { get; set; }
        public string Ram { get; set; }
        public string Rom { get; set; }
        public string Color { get; set; }
        public string Display { get; set; }
        public string Size { get; set; }
        public string BackCamera { get; set; }
        public string FrontCamera { get; set; }
        public string Battery { get; set; }
        public bool? IsSale { get; set; }
        public decimal CurrentPrice { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
