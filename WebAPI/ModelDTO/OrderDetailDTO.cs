using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.ModelDTO
{
    public class OrderDetailDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsSale { get; set; }
        public decimal CurrentPrice { get; set; }

        public static List<OrderDetailDTO> CreateList(List<OrderDetail> list)
        {
            List<OrderDetailDTO> DetailList = new List<OrderDetailDTO>();
            foreach(var item in list)
            {
                OrderDetailDTO index = new OrderDetailDTO();
                index.ProductId = item.ProductId.GetValueOrDefault();
                index.ProductName = item.Product.Name;
                index.Quantity = item.Quantity.GetValueOrDefault();
                index.Price = item.Price;
                index.IsSale = item.IsSale.GetValueOrDefault();
                index.CurrentPrice = item.CurrentPrice;
                DetailList.Add(index);
            }
            return DetailList;
        }
    }
}
