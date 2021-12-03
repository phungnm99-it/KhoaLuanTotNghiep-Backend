using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;

namespace WebAPI.ModelDTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public decimal TotalCost { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime OrderTime { get; set; }

        public List<OrderDetailDTO> Products { get; set; }
    }
}
