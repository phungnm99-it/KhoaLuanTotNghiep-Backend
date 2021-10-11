using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ModelDTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsSale { get; set; }
        public bool IsFeatured { get; set; }
        public decimal CurrentPrice { get; set; }
        public string BrandName { get; set; }
        public string Cpu { get; set; }
        public string Color { get; set; }
        public string OS { get; set; }
        public string RAM { get; set; }
        public string ROM { get; set; }
        public string Display { get; set; }
        public string Size { get; set; }
        public string BackCamera { get; set; }
        public string FrontCamera { get; set; }
        public string Battery { get; set; }
    }
}
