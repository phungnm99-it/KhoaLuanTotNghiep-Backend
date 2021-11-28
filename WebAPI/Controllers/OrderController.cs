using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
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
            orderModel.UserId = user.Id;
            var result = await _orderService.CreateOrder(orderModel);
            if (result == false)
                return new ObjectResult(new { code = 401, message = "Failed" });
            return new ObjectResult(new { code = 200, data = result });
        }

        [Route("getAllOwnOrders")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrdersOfCurrentUser()
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _orderService.GetOwnerOrders(user.Id);
            return new ObjectResult(new { code = 200, data = result });
        }
    }
}
