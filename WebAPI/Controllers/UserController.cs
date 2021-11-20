using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Helper;
using WebAPI.ModelDTO;
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
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            var user = await _userService.AuthenticateAsync(model.Username, model.Password);
            if (user == null)
            {
                return new ObjectResult(new { code = "401", message = "Username or password is wrong!" });
            }
            var token = _jwtUtils.GenerateToken(user);

            return new ObjectResult(new { code = "200", token = token });
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            var user = await _userService.RegisterAsync(model);
            if (user == null)
                return new ObjectResult(new { code = "401", message = "Email or username exist" });

            return new ObjectResult(new { code = "200", user = user });
        }

        [AllowAnonymous]
        [Route("loginWithGoogle")]
        [HttpPost]
        public async Task<IActionResult> LoginWithGoogle([FromBody] JsonElement tokenId)
        {
            string token = JsonSerializer.Serialize(tokenId);
            var payload = await _jwtUtils.VerifyGoogleToken(token.Substring(1, token.Length - 2));
            if (payload == null)
                return BadRequest("Invalid External Authentication.");

            var user = await _userService.AuthenticateGoogleAsync(payload);

            var generatedToken = _jwtUtils.GenerateToken(user);
            return Ok(new { code = "200", token = generatedToken });
        }


        [Authorize(Roles = Role.Admins)]
        [Route("getAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _userService.GetAllUserAsync();
            return new OkObjectResult(new { code = "200", data = list });
        }

        [Authorize(Roles = Role.Admins)]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            return new OkObjectResult(new { code = "200", data = user });
        }

        [Route("getUser")]
        [HttpGet]
        public IActionResult GetUser()
        {
            return new OkObjectResult(new { code = "200", data = HttpContext.Items["User"] });
        }

        [Authorize(Roles = Role.SuperAdmin)]
        [Route("createAdminAccount")]
        [HttpPost]
        public async Task<IActionResult> CreateAdminAccount([FromForm] RegisterModel model)
        {
            var user = await _userService.AddAdminAccount(model);
            if (user == null)
            {
                return new ObjectResult(new { code = 401, message = "Email exists" });
            }
            return new ObjectResult(new { code = 200, data = user });
        }

        [Route("changePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] string newPassword)
        {
            var userId = (HttpContext.Items["User"] as UserDTO).Id;
            await _userService.ChangePasswordAsync(userId, newPassword);
            return new ObjectResult(new { code = 200 });
        }

        [AllowAnonymous]
        [Route("resetPassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordModel model)
        {
            var result = await _userService.ResetNewPassword(model);
            if (!result)
                return new ObjectResult(new { code = "401", message = "Error" });

            return new ObjectResult(new { code = "200", message = "Success" });
        }

        [Route("uploadImage")]
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm]IFormFile image)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _userService.UploadImage(image, user.Id);
            if (!result)
                return new ObjectResult(new { code = "401", message = "Error" });

            return new ObjectResult(new { code = "200", message = "Success" });
        }
    }
}
