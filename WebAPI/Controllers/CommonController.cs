using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebAPI.Helper;
using WebAPI.RepositoryService.Interface;

namespace WebAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : Controller
    {
        private IOrderService _orderService;
        
        public CommonController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //[Authorize(Roles =RoleHelper.Admin)]
        [Route("caculateTotal")]
        [HttpGet]
        public async Task<IActionResult> GetCaculateTotalAsync()
        {
            var data = await _orderService.CaculateTotalAsync();
            return new OkObjectResult(new { code = "200", data = data });
        }

        //[Authorize(Roles =RoleHelper.Admin)]
        [Route("caculateOrder")]
        [HttpGet]
        public async Task<IActionResult> GetCaculateOrderAsync()
        {
            var data = await _orderService.CaculateOrderAsync();
            return new OkObjectResult(new { code = "200", data = data });
        }
    }
}
