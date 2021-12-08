using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Repository.Interface
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetOrderDetailByOrderIdAsync(int id);
        void CreateOrderDetail(OrderDetail orderDetail);
        Task<IEnumerable<OrderDetail>> GetOrderDetailByProductId(int productId);

        Task<IEnumerable<OrderDetail>> GetAll();
    }
}
