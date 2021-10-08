using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Helper;
using WebAPI.RepositoryService.Interface;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IUserService _userService { get; set; }
        private IJwtUtils _jwtUtils { get; set; }
        public UserController(IUserService userService, IJwtUtils jwtUtils)
        {
            _userService = userService;
            _jwtUtils = jwtUtils;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return new OkObjectResult(new { code = "200", message = "Username or password wrong!" });
            }
            var token = _jwtUtils.GenerateToken(user);

            return new OkObjectResult(new { code = "200", token = token });
        }

        [AllowAnonymous]
        [Route("loginwithgoogle")]
        [HttpPost]
        public async Task<IActionResult> LoginWithGoogle([FromBody]JsonElement body)
        {
            string token = JsonSerializer.Serialize(body);
            var payload = await _jwtUtils.VerifyGoogleToken(token.Substring(1,token.Length-2));
            if (payload == null)
                return BadRequest("Invalid External Authentication.");

            var user = _userService.Authenticate("user", "user");

            var generatedToken = _jwtUtils.GenerateToken(user);
            return Ok(new { code = "200", token = generatedToken, email = payload.Email});
        }

        [Authorize(Roles = Role.Admin)]
        [Route("getall")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return new OkObjectResult(new { code = "200", data = _userService.GetAll() });
        }


        [Authorize(Roles = Role.Admin)]
        [Route("getbyid")]
        [HttpGet]
        public IActionResult GetById(int id)
        {
            return new OkObjectResult(new { code = "200", data = _userService.GetById(id) });
        }

        [Authorize(Roles = Role.Admin + "," + Role.User)]
        [Route("getuser")]
        [HttpGet]
        public IActionResult GetUser()
        {
            return new OkObjectResult(new { code = "200", data = HttpContext.Items["User"] });
        }

    }
}
