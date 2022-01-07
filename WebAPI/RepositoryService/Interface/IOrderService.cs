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
        Task<bool> CreateOrderAsync(OrderModel order);

        Task<bool> CreateOrderWithPaypalAsync(OrderModel order);
        Task<List<OrderDTO>> GetOwnerOrdersAsync(int userId);
        Task<bool> VerifyOrderByAdminAsync(int orderId, int adminId);
        Task<bool> CompleteOrderByShipperAsync(int orderId, int shipperId);

        Task<bool> DeliverOrderByShipperAsync(int orderId, int shipperId);

        Task<List<OrderDTO>> GetOrderCanDeliverByShipperAsync(int shipperId);
        Task<List<OrderDTO>> GetOrderDeliveringByShipperAsync(int shipperId);
        Task<List<OrderDTO>> GetOrderDeliveredByShipperAsync(int shipperId);
        Task<bool> CancelOrderByUserAsync(int orderId, int userId);

        Task<bool> CancelOrderByAdminAsync(int orderId, int adminId);

        Task<bool> CancelOrderByShipperAsync(int orderId, int shipperId);

        Task<int[]> CaculateOrderAsync();
        Task<TotalDTO> CaculateTotalAsync();
    }
}
