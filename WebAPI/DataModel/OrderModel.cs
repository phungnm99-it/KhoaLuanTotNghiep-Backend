using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DataModel
{
    public class OrderModel
    {
        public int UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; }
        public string Address { get; set; }
        public List<ProductOrderModel> ProductList { get; set; }
    }

    public class ProductOrderModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }

}
