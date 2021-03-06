using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Helper;
using WebAPI.ModelDTO;
using WebAPI.Models;
using WebAPI.RepositoryService.Interface;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel orderModel)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            if (user == null)
                return new UnauthorizedObjectResult(new { code = 401, message = false });

            if(orderModel.ProductList?.Any() != true)
                return new ObjectResult(new { code = 401, message = "Failed" });

            orderModel.UserId = user.Id;
            orderModel.PaymentMethod = "Thanh toán trực tiếp";
            var result = await _orderService.CreateOrderAsync(orderModel);
            if (result == false)
                return new ObjectResult(new { code = 401, message = "Failed" });
            return new ObjectResult(new { code = 200, data = result });
        }

        [Route("createPaypal")]
        [HttpPost]
        public async Task<IActionResult> CreateOrderPaypal([FromBody] OrderModel orderModel)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            if (user == null)
                return new UnauthorizedObjectResult(new { code = 401, message = false });

            if (orderModel.ProductList?.Any() != true)
                return new ObjectResult(new { code = 401, message = "Failed" });

            orderModel.UserId = user.Id;
            orderModel.PaymentMethod = "Paypal";
            var result = await _orderService.CreateOrderAsync(orderModel);
            if (result == false)
                return new ObjectResult(new { code = 401, message = "Failed" });
            return new ObjectResult(new { code = 200, data = result });
        }

        [Route("getAllOwnOrders")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrdersOfCurrentUser()
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _orderService.GetOwnerOrdersAsync(user.Id);
            return new ObjectResult(new { code = 200, data = result });
        }


        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _orderService.GetOrderByIdAsync(id, user.Id, user.RoleName);
            return new ObjectResult(new { code = 200, data = result });
        }

        [Authorize(Roles =RoleHelper.Admins)]
        [Route("getAll")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return new ObjectResult(new { code = 200, data = result });
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("verify/{id}")]
        [HttpGet]
        public async Task<IActionResult> VerifyOrderByAdmin(int id)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _orderService.VerifyOrderByAdminAsync(id, user.Id);
            if (result == false)
                return new ObjectResult(new { code = 401, message = "Failed" });
            return new ObjectResult(new { code = 200, message = "Success" });
        }

        [Authorize(Roles = RoleHelper.Shipper)]
        [Route("complete/{id}")]
        [HttpGet]
        public async Task<IActionResult> CompleteOrderByShipper(int id)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _orderService.CompleteOrderByShipperAsync(id, user.Id);
            if (result == false)
                return new ObjectResult(new { code = 401, message = "Failed" });
            return new ObjectResult(new { code = 200, message = "Success" });
        }

        [Authorize(Roles = RoleHelper.Shipper)]
        [Route("deliver/{id}")]
        [HttpGet]
        public async Task<IActionResult> DeliverOrderByShipper(int id)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _orderService.DeliverOrderByShipperAsync(id, user.Id);
            if (result == false)
                return new ObjectResult(new { code = 401, message = "Failed" });
            return new ObjectResult(new { code = 200, message = "Success" });
        }

        [Authorize(Roles = RoleHelper.Shipper)]
        [Route("shipperCancel/{id}")]
        [HttpGet]
        public async Task<IActionResult> CancelOrderByShipper(int id)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _orderService.CancelOrderByShipperAsync(id, user.Id);
            if (result == false)
                return new ObjectResult(new { code = 401, message = "Failed" });
            return new ObjectResult(new { code = 200, message = "Success" });
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("adminCancel/{id}")]
        [HttpGet]
        public async Task<IActionResult> CancelOrderByAdmin(int id)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _orderService.CancelOrderByAdminAsync(id, user.Id);
            if (result == false)
                return new ObjectResult(new { code = 401, message = "Failed" });
            return new ObjectResult(new { code = 200, message = "Success" });
        }

        [Authorize(Roles = RoleHelper.User)]
        [Route("userCancel/{id}")]
        [HttpGet]
        public async Task<IActionResult> CancelOrderByUser(int id)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _orderService.CancelOrderByUserAsync(id, user.Id);
            if (result == false)
                return new ObjectResult(new { code = 401, message = "Failed" });
            return new ObjectResult(new { code = 200, message = "Success" });
        }

        [Authorize(Roles = RoleHelper.Shipper)]
        [Route("delivering")]
        [HttpGet]
        public async Task<IActionResult> GetOrderDeliveringByShipper()
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _orderService.GetOrderDeliveringByShipperAsync(user.Id);
            return new ObjectResult(new { code = 200, data = result });
        }

        [Authorize(Roles = RoleHelper.Shipper)]
        [Route("delivered")]
        [HttpGet]
        public async Task<IActionResult> GetOrderDeliveredByShipper()
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _orderService.GetOrderDeliveredByShipperAsync(user.Id);
            return new ObjectResult(new { code = 200, data = result });
        }

        [Authorize(Roles = RoleHelper.Shipper)]
        [Route("canDeliver")]
        [HttpGet]
        public async Task<IActionResult> GetOrderCanDeliverByShipper()
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _orderService.GetOrderCanDeliverByShipperAsync(user.Id);
            return new ObjectResult(new { code = 200, data = result });
        }
    }
}
