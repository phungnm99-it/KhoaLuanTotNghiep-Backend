using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Repository.Interface
{
    public interface IStatusUpdateOrder
    {
        Task<StatusUpdateOrder> GetStatusUpdateOrderByOrderIdAsync(int orderId);
        void CreateStatusUpdateOrder(StatusUpdateOrder model);
        void UpdateStatusUpdateOrder(StatusUpdateOrder model);
    }
}
