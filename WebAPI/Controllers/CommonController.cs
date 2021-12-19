using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : Controller
    {
        [Route("currencyRate")]
        [HttpGet]
        public async Task<IActionResult> GetRate()
        {
            return new OkObjectResult(new { code = "200", rate = 23000 });
        }
    }
}
