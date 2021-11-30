using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.ModelDTO;

namespace WebAPI.RepositoryService.Interface
{
    public interface IOrderService
    {
        Task<OrderDTO> GetOrderByIdAsync(int id, int userId, string role);
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<bool> CreateOrder(OrderModel order);
        Task<List<OrderDTO>> GetOwnerOrders(int userId);
        Task<bool> VerifyOrderByAdminAsync(int orderId, int adminId);
        Task<bool> CompleteOrderByShipper(int orderId, int shipperId);
    }
}
