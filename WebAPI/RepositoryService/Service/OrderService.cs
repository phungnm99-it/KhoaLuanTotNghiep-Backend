using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> CancelOrderByShipperAsync(int orderId, int shipperId)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetOrderByIdAsync(orderId);
                if (order == null || !order.Status.Equals("Đang chờ giao hàng"))
                    return false;

                order.Status = "Đã huỷ";

                StatusUpdateOrder st = new StatusUpdateOrder()
                {
                    OrderId = orderId,
                    UpdatedBy = shipperId,
                    Detail = "Shipper huỷ đơn hàng",
                    UpdatedTime = DateTime.Now
                };
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

        public async Task<bool> CancelOrderByUserAsync(int orderId, int userId)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetOrderByIdAsync(orderId);
                if (order == null || (!order.Status.Equals("Đặt hàng thành công") && !order.Status.Equals("Đã xác nhận")))
                    return false;

                order.Status = "Đã huỷ";

                StatusUpdateOrder st = new StatusUpdateOrder()
                {
                    OrderId = orderId,
                    UpdatedBy = userId,
                    Detail = "User huỷ đơn hàng",
                    UpdatedTime = DateTime.Now
                };
                order.UpdatedTime = DateTime.Now;
                order.UpdatedBy = userId;
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

        public async Task<bool> CompleteOrderByShipperAsync(int orderId, int shipperId)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetOrderByIdAsync(orderId);
                if (order == null || !order.Status.Equals("Đang chờ giao hàng")) 
                    return false;

                var ordes = await _unitOfWork.Orders.FindByCondition(od => od.ShipperId == shipperId && od.IsCompleted != true)
                    .ToListAsync();
                if (ordes.Count >= 10)
                    return false;

                StatusUpdateOrder st = new StatusUpdateOrder()
                {
                    OrderId = orderId,
                    UpdatedBy = shipperId,
                    Detail = "Shipper giao thành công",
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

        public async Task<bool> CreateOrderAsync(OrderModel order)
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
                model.Name = order.Name;
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

        public async Task<bool> DeliverOrderByShipperAsync(int orderId, int shipperId)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetOrderByIdAsync(orderId);
                if (order == null || !order.Status.Equals("Đã xác nhận"))
                    return false;
                StatusUpdateOrder st = new StatusUpdateOrder()
                {
                    OrderId = orderId,
                    UpdatedBy = shipperId,
                    Detail = "Shipper nhận đơn",
                    UpdatedTime = DateTime.Now
                };

                order.ShipperId = shipperId;
                order.Status = "Đang chờ giao hàng";
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

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllOrdersAsync();
            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            foreach(var item in orders)
            {
                OrderDTO orderDTO = new OrderDTO
                {
                    Id = item.Id,
                    Address = item.Address,
                    OrderCode = item.OrderCode,
                    PaymentMethod = item.PaymentMethod,
                    PhoneNumber = item.PhoneNumber,
                    Name = item.Name,
                    Status = item.Status,
                    TotalCost = item.TotalCost
                };
                orderDTO.Products = new List<OrderDetailDTO>();
                var orderDetail = await _unitOfWork.OrderDetails.GetOrderDetailByOrderIdAsync(orderDTO.Id);
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
                orderDTOs.Add(orderDTO);
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
                Name = order.Name,
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

        public async Task<List<OrderDTO>> GetOrderCanDeliverByShipperAsync(int shipperId)
        {
            var orders = _unitOfWork.Orders.FindByCondition(order => order.ShipperId == shipperId || order.IsCompleted == false).ToList();
            if (orders.Count() == 10)
                return null;

            var orderlist = _unitOfWork.Orders.FindByCondition(order => order.Status == "Đã xác nhận").ToList();

            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            foreach (var item in orderlist)
            {
                OrderDTO orderDTO = new OrderDTO()
                {
                    Id = item.Id,
                    Address = item.Address,
                    OrderCode = item.OrderCode,
                    PaymentMethod = item.PaymentMethod,
                    PhoneNumber = item.PhoneNumber,
                    Name = item.Name,
                    TotalCost = item.TotalCost,
                    OrderTime = item.OrderTime,
                    Status = "2"
                };

                orderDTO.Products = new List<OrderDetailDTO>();
                var orderDetail = await _unitOfWork.OrderDetails.GetOrderDetailByOrderIdAsync(item.Id);
                foreach (var detail in orderDetail)
                {
                    OrderDetailDTO de = new OrderDetailDTO()
                    {
                        ProductId = detail.ProductId.GetValueOrDefault(),
                        ProductName = detail.Product.Name,
                        Quantity = detail.Quantity.GetValueOrDefault(),
                        Price = detail.Price,
                        IsSale = detail.IsSale.GetValueOrDefault(),
                        CurrentPrice = detail.CurrentPrice,
                        ImageUrl = detail.Product.ImageUrl
                    };
                    orderDTO.Products.Add(de);
                }
                orderDTOs.Add(orderDTO);
            }
            return orderDTOs;
        }

        public async Task<List<OrderDTO>> GetOrderDeliveredByShipperAsync(int shipperId)
        {
            var orderlist = _unitOfWork.Orders.FindByCondition(order => order.ShipperId == shipperId).ToList();

            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            foreach (var item in orderlist)
            {
                OrderDTO orderDTO = new OrderDTO()
                {
                    Id = item.Id,
                    Address = item.Address,
                    OrderCode = item.OrderCode,
                    PaymentMethod = item.PaymentMethod,
                    PhoneNumber = item.PhoneNumber,
                    Name = item.Name,
                    TotalCost = item.TotalCost,
                    OrderTime = item.OrderTime,
                    Status = "4"
                };

                orderDTO.Products = new List<OrderDetailDTO>();
                var orderDetail = await _unitOfWork.OrderDetails.GetOrderDetailByOrderIdAsync(item.Id);
                foreach (var detail in orderDetail)
                {
                    OrderDetailDTO de = new OrderDetailDTO()
                    {
                        ProductId = detail.ProductId.GetValueOrDefault(),
                        ProductName = detail.Product.Name,
                        Quantity = detail.Quantity.GetValueOrDefault(),
                        Price = detail.Price,
                        IsSale = detail.IsSale.GetValueOrDefault(),
                        CurrentPrice = detail.CurrentPrice,
                        ImageUrl = detail.Product.ImageUrl
                    };
                    orderDTO.Products.Add(de);
                }
                orderDTOs.Add(orderDTO);
            }
            return orderDTOs;
        }

        public async Task<List<OrderDTO>> GetOrderDeliveringByShipperAsync(int shipperId)
        {
            var orderlist = _unitOfWork.Orders.FindByCondition(order => order.Status == "Đang chờ giao hàng"
            && order.ShipperId == shipperId).ToList();

            List<OrderDTO> orderDTOs = new List<OrderDTO>();
            foreach (var item in orderlist)
            {
                OrderDTO orderDTO = new OrderDTO()
                {
                    Id = item.Id,
                    Address = item.Address,
                    OrderCode = item.OrderCode,
                    PaymentMethod = item.PaymentMethod,
                    PhoneNumber = item.PhoneNumber,
                    Name = item.Name,
                    TotalCost = item.TotalCost,
                    OrderTime = item.OrderTime,
                    Status = "3"
                };

                orderDTO.Products = new List<OrderDetailDTO>();
                var orderDetail = await _unitOfWork.OrderDetails.GetOrderDetailByOrderIdAsync(item.Id);
                foreach (var detail in orderDetail)
                {
                    OrderDetailDTO de = new OrderDetailDTO()
                    {
                        ProductId = detail.ProductId.GetValueOrDefault(),
                        ProductName = detail.Product.Name,
                        Quantity = detail.Quantity.GetValueOrDefault(),
                        Price = detail.Price,
                        IsSale = detail.IsSale.GetValueOrDefault(),
                        CurrentPrice = detail.CurrentPrice,
                        ImageUrl = detail.Product.ImageUrl
                    };
                    orderDTO.Products.Add(de);
                }
                orderDTOs.Add(orderDTO);
            }
            return orderDTOs;
        }

        public async Task<List<OrderDTO>> GetOwnerOrdersAsync(int userId)
        {
            var orders = _unitOfWork.Orders.FindByCondition(order => order.UserId == userId)
                .OrderByDescending(order => order.OrderTime).ToList();
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
                    Name = item.Name,
                    TotalCost = item.TotalCost,
                    OrderTime = item.OrderTime,
                    Status = item.Status
                };

                if(orderDTO.Status.Equals("Đặt hàng thành công"))
                {
                    orderDTO.Status = "1";
                }
                else if (orderDTO.Status.Equals("Đã xác nhận"))
                {
                    orderDTO.Status = "2";
                }
                else if (orderDTO.Status.Equals("Đang chờ giao hàng"))
                {
                    orderDTO.Status = "3";
                }
                else if (orderDTO.Status.Equals("Giao hàng thành công"))
                {
                    orderDTO.Status = "4";
                }
                else
                {
                    orderDTO.Status = "0";
                }

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
                        CurrentPrice = detail.CurrentPrice,
                         ImageUrl = detail.Product.ImageUrl
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
                if (order == null || !order.Status.Equals("Đặt hàng thành công"))
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
