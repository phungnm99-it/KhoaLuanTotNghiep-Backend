using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DataModel
{
    public class OrderModel
    {
        public string UserId { get; set; }
        public string Address { get; set; }
        public List<ProductModel> ListProducts { get; set; }
    }
}
