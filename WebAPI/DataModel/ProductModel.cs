using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DataModel
{
    public class ProductModel
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string BrandName { get; set; }
        public string Cpu { get; set; }
        public string Gpu { get; set; }
        public string ScreenResolution { get; set; }
        public string Os { get; set; }
        public string Ram { get; set; }
        public string Rom { get; set; }
        public string Color { get; set; }
        public string ScreenTech { get; set; }
        public string ScreenSize { get; set; }
        public string BackCamera { get; set; }
        public string FrontCamera { get; set; }
        public string Battery { get; set; }
        public string Sim { get; set; }
        public string Wifi { get; set; }
        public string Gps { get; set; }
    }
}
