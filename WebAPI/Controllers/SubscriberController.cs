using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebAPI.RepositoryService.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriberController : Controller
    {
        private ISubscriberService service;
        public SubscriberController(ISubscriberService service)
        {
            this.service = service;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await service.GetAllSubscribersAsync();
            return new OkObjectResult(new { code = "200", data = list });
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetSubsriberById(int id)
        {
            var subscriber = await service.GetSubscriberByIdAsync(id);
            if(subscriber == null)
            {
                return new OkObjectResult(new { code = 401, message = "Error Id" });
            }
            return new OkObjectResult(new { code = "200", data = subscriber });
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddSubscriber([FromForm] string email)
        {
            var subscriber = await service.AddSubscriberAsync(email);
            if (subscriber == null)
                return new OkObjectResult(new { code = 401, message = "fail" });

            return new OkObjectResult(new { code = "200", data = subscriber });
        }

        [Route("remove")]
        [HttpPut]
        public async Task<IActionResult> RemoveSubscriber([FromForm] int id)
        {
            var subscriber = await service.RemoveSubscriberAsync(id);
            if (subscriber == false)
                return new OkObjectResult(new { code = 401, message = "fail" });

            return new OkObjectResult(new { code = "200", message = "success" });
        }
    }
}
