using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Helper;
using WebAPI.ModelDTO;
using WebAPI.RepositoryService.Interface;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService service)
        {
            _feedbackService = service;
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _feedbackService.GetAllFeedbacksAsync();
            return new OkObjectResult(new { code = "200", data = list });
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("getFeedbackHasReply")]
        [HttpGet]
        public async Task<IActionResult> GetFeedbackHasReply()
        {
            var list = await _feedbackService.GetFeedbackHasReply();
            return new OkObjectResult(new { code = "200", data = list });
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("getFeedbackNoReply")]
        [HttpGet]
        public async Task<IActionResult> GetFeedbackNoReply()
        {
            var list = await _feedbackService.GetFeedbackNoReply();
            return new OkObjectResult(new { code = "200", data = list });
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            return new OkObjectResult(new { code = "200", data = feedback });
        }

        [AllowAnonymous]
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] FeedbackModel model)
        {
            var feedback = await _feedbackService.CreateFeedbackAsync(model);
            if (feedback == null)
            {
                return new ObjectResult(new { code = "401", message = "Error!" });
            }
            return new OkObjectResult(new { code = "200", data = feedback });
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("reply")]
        [HttpPost]
        public async Task<IActionResult> Reply([FromForm] ReplyFeedbackModel model)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var feedback = await _feedbackService.ReplyFeedbackAsync(model, user.Id);
            if(feedback == null)
            {
                return new ObjectResult(new { code = "401", message = "Error!" });
            }
            return new OkObjectResult(new { code = "200", data = feedback });
        }


    }
}
