using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repository.Interface;

namespace WebAPI.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(PTStoreContext context) : base(context) { }

        public void CreateOrder(Order order)
        {
            Create(order);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await FindByCondition(order => order.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<Order> GetOrderByOrderCodeAsync(string orderCode)
        {
            return await FindByCondition(order => order.OrderCode == orderCode).FirstOrDefaultAsync();
        }

        public void UpdateOrder(Order order)
        {
            Update(order);
        }
    }
}
