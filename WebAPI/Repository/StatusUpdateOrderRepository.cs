using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repository.Interface;

namespace WebAPI.Repository
{
    public class StatusUpdateOrderRepository : GenericRepository<StatusUpdateOrder>, IStatusUpdateOrder
    {
        public StatusUpdateOrderRepository(PTStoreContext context) : base(context) { }

        public void CreateStatusUpdateOrder(StatusUpdateOrder model)
        {
            Create(model);
        }

        public async Task<StatusUpdateOrder> GetStatusUpdateOrderByOrderIdAsync(int orderId)
        {
            return await FindByCondition(st => st.OrderId == orderId).FirstOrDefaultAsync();
        }

        public void UpdateStatusUpdateOrder(StatusUpdateOrder model)
        {
            Update(model);
        }
    }
}
