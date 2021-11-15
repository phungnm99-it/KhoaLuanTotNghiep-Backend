using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Repository.Interface
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
    }
}
