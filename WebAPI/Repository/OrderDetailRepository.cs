using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repository.Interface;

namespace WebAPI.Repository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(PTStoreContext context) : base(context) { }

        public void CreateOrderDetail(OrderDetail orderDetail)
        {
            Create(orderDetail);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailByOrderIdAsync(int id)
        {
            return await FindByCondition(detail => detail.OrderId == id).Include(detail => detail.Product)
                .Include(detail => detail.Order).ToListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailByProductId(int productId)
        {
            return await FindByCondition(dt => dt.ProductId == productId)
                .Include(dt => dt.Order).ToListAsync();
        }
    }
}
