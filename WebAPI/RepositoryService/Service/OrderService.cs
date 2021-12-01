using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.ModelDTO;
using WebAPI.Models;
using WebAPI.RepositoryService.Interface;
using WebAPI.UnitOfWorks;

namespace WebAPI.RepositoryService.Service
{
    public class OrderService : IOrderService
    {
        private IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CompleteOrderByShipper(int orderId, int shipperId)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetOrderByIdAsync(orderId);
                if (order == null || order.IsCompleted == true || order.Status.Equals("Giao hàng thành công")) 
                    return false;
                StatusUpdateOrder st = new StatusUpdateOrder()
                {
                    OrderId = orderId,
                    UpdatedBy = shipperId,
                    Detail = "Giao hàng thành công",
                    UpdatedTime = DateTime.Now
                };

                order.Status = "Giao hàng thành công";
                order.IsCompleted = true;
                order.UpdatedTime = DateTime.Now;
                order.UpdatedBy = shipperId;
                _unitOfWork.Orders.UpdateOrder(order);
                _unitOfWork.StatusUpdateOrders.CreateStatusUpdateOrder(st);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CreateOrder(OrderModel order)
        {
            foreach(var item in order.ProductList)
            {
                var product = await _unitOfWork.Products.GetProductByIdAsync(item.Id);
                if(product.Stock < item.Quantity)
                {
                    return false;
                }
            }

            DateTime datetime = new DateTime();
            try
            {
                Order model = new Order();
                model.OrderCode = "DH" + DateTime.Now.Day.ToString()
                    + DateTime.Now.Hour.ToString()
                    + DateTime.Now.Minute.ToString() + order.UserId.ToString();
                model.UserId = order.UserId;
                model.Address = order.Address;
                model.PhoneNumber = order.PhoneNumber;
                model.Status = "Đặt hàng thành công";
                model.IsCompleted = false;
                model.PaymentMethod = order.PaymentMethod;
                model.OrderTime = DateTime.Now;
                datetime = model.OrderTime;
                model.UpdatedTime = DateTime.Now;
                model.UpdatedBy = order.UserId;
                model.TotalCost = 0;
                foreach (var item in order.ProductList)
                {
                    var product = await _unitOfWork.Products.GetProductByIdAsync(item.Id);
                    model.TotalCost += product.CurrentPrice * item.Quantity;
                }
                _unitOfWork.Orders.CreateOrder(model);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return false;
            }

            try
            {
                var orderFind = _unitOfWork.Orders.FindByCondition(ord => ord.OrderTime == datetime &&
                ord.UserId == order.UserId).FirstOrDefault();
                foreach (var item in order.ProductList)
                {
                    var product = await _unitOfWork.Products.GetProductByIdAsync(item.Id);
                    OrderDetail detail = new OrderDetail();
                    detail.OrderId = orderFind.Id;
                    detail.ProductId = product.Id;
                    detail.Quantity = item.Quantity;
                    product.Stock -= item.Quantity;
                    if (product.Stock == 0)
                    {
                        product.Status = "Hết hàng";
                    }
                    _unitOfWork.Products.UpdateProduct(product);

                    detail.Price = product.Price;
                    detail.IsSale = product.IsSale;
                    detail.CurrentPrice = product.CurrentPrice;
                    _unitOfWork.OrderDetails.CreateOrderDetail(detail);
                }
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllOrdersAsync();
            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            foreach(var item in orders)
            {
                orderDTOs.Add(new OrderDTO
                {
                    Id = item.Id,
                    Address = item.Address,
                    OrderCode = item.OrderCode,
                    PaymentMethod = item.PaymentMethod,
                    PhoneNumber = item.PhoneNumber,
                    Status = item.Status,
                    TotalCost = item.TotalCost
                });
            }
            return orderDTOs;
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int id, int userId, string role)
        {
            var order = await _unitOfWork.Orders.GetOrderByIdAsync(id);
            if (order == null)
                return null;
            if (role == Helper.RoleHelper.User && order.UserId != userId)
                return null;
            OrderDTO orderDTO = new OrderDTO()
            {
                Id = order.Id,
                Address = order.Address,
                OrderCode = order.OrderCode,
                PaymentMethod = order.PaymentMethod,
                PhoneNumber = order.PhoneNumber,
                TotalCost = order.TotalCost,
                Status = order.Status
            };

            orderDTO.Products = new List<OrderDetailDTO>();
            var orderDetail = await _unitOfWork.OrderDetails.GetOrderDetailByOrderIdAsync(order.Id);
            foreach (var detail in orderDetail)
            {
                OrderDetailDTO de = new OrderDetailDTO()
                {
                    ProductId = detail.ProductId.GetValueOrDefault(),
                    ProductName = detail.Product.Name,
                    Quantity = detail.Quantity.GetValueOrDefault(),
                    Price = detail.Price,
                    IsSale = detail.IsSale.GetValueOrDefault(),
                    CurrentPrice = detail.CurrentPrice
                };
                orderDTO.Products.Add(de);
            }
            return orderDTO;
        }

        public async Task<List<OrderDTO>> GetOwnerOrders(int userId)
        {
            var orders = _unitOfWork.Orders.FindByCondition(order => order.UserId == userId).ToList();
            if (orders == null)
                return null;

            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            foreach(var item in orders)
            {
                OrderDTO orderDTO = new OrderDTO()
                {
                    Id = item.Id,
                    Address = item.Address,
                    OrderCode = item.OrderCode,
                    PaymentMethod = item.PaymentMethod,
                    PhoneNumber = item.PhoneNumber,
                    TotalCost = item.TotalCost,
                    OrderTime = item.OrderTime,
                    Status = item.Status
                };
                orderDTO.Products = new List<OrderDetailDTO>();
                var orderDetail = await _unitOfWork.OrderDetails.GetOrderDetailByOrderIdAsync(item.Id);
                foreach(var detail in orderDetail)
                {
                    OrderDetailDTO de = new OrderDetailDTO()
                    {
                        ProductId = detail.ProductId.GetValueOrDefault(),
                        ProductName = detail.Product.Name,
                        Quantity = detail.Quantity.GetValueOrDefault(),
                        Price = detail.Price,
                        IsSale = detail.IsSale.GetValueOrDefault(),
                        CurrentPrice = detail.CurrentPrice
                    };
                    orderDTO.Products.Add(de);
                }
                orderDTOs.Add(orderDTO);
            }
            return orderDTOs;
        }

        public async Task<bool> VerifyOrderByAdminAsync(int orderId, int adminId)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetOrderByIdAsync(orderId);
                if (order == null || order.IsCompleted == true || order.Status.Equals("Đã xác nhận"))
                    return false;
                StatusUpdateOrder st = new StatusUpdateOrder()
                {
                    OrderId = orderId,
                    UpdatedBy = adminId,
                    Detail = "Xác nhận đơn hàng",
                    UpdatedTime = DateTime.Now
                };

                order.Status = "Đã xác nhận";
                order.UpdatedTime = DateTime.Now;
                order.UpdatedBy = adminId;
                _unitOfWork.Orders.UpdateOrder(order);
                _unitOfWork.StatusUpdateOrders.CreateStatusUpdateOrder(st);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
