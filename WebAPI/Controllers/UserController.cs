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
        [Route("loginAdmin")]
        [HttpPost]
        public async Task<IActionResult> LoginAdmin([FromForm] LoginModel model)
        {
            var user = await _userService.AuthenticateAdminAsync(model.Username, model.Password);
            if (user == null)
            {
                return new ObjectResult(new { code = "401", message = "Username or password is wrong!" });
            }
            var token = _jwtUtils.GenerateToken(user);

            return new ObjectResult(new { code = "200", token = token });
        }

        [AllowAnonymous]
        [Route("loginShipper")]
        [HttpPost]
        public async Task<IActionResult> LoginShipper([FromForm] LoginModel model)
        {
            var user = await _userService.AuthenticateShipperAsync(model.Username, model.Password);
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
        public async Task<IActionResult> LoginWithGoogle([FromForm] string tokenId)
        {
            //string token = JsonSerializer.Serialize(tokenId);
            //var payload = await _jwtUtils.VerifyGoogleToken(token.Substring(1, token.Length - 2));
            var payload = await _jwtUtils.VerifyGoogleToken(tokenId);
            if (payload == null)
                return new ObjectResult(new { code = "401" });

            var user = await _userService.AuthenticateGoogleAsync(payload);

            var generatedToken = _jwtUtils.GenerateToken(user);
            return new ObjectResult(new { code = "200", token = generatedToken });
        }


        [Authorize(Roles = RoleHelper.Admins)]
        [Route("getAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _userService.GetAllUserAsync();
            return new OkObjectResult(new { code = "200", data = list });
        }

        [Authorize(Roles = RoleHelper.SuperAdmin)]
        [Route("getAllAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAllAdminAsync()
        {
            var list = await _userService.GetAllAdminAsync();
            return new OkObjectResult(new { code = "200", data = list });
        }

        [Authorize(Roles = RoleHelper.SuperAdmin)]
        [Route("resetPassword/{id}")]
        [HttpGet]
        public async Task<IActionResult> ResetPasswordAsync(int id)
        {
            var rs = await _userService.ResetPasswordAsync(id);
            if(rs == false)
            {
                return new OkObjectResult(new { code = 401, message = "Fail" });
            }
            return new OkObjectResult(new { code = 200, message = "Success" });
        }

        [Authorize(Roles = RoleHelper.SuperAdmin)]
        [Route("getAllShipper")]
        [HttpGet]
        public async Task<IActionResult> GetAllShipperAsync()
        {
            var list = await _userService.GetAllShipperAsync();
            return new OkObjectResult(new { code = "200", data = list });
        }

        [Authorize(Roles = RoleHelper.User)]
        [Route("getReview")]
        [HttpGet]
        public async Task<IActionResult> GetAllReview()
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var list = await _userService.GetAllOwnReviewsAsync(user.Id);
            return new OkObjectResult(new { code = "200", data = list });
        }

        [Authorize(Roles = RoleHelper.Admins)]
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

        [Authorize(Roles = RoleHelper.SuperAdmin)]
        [Route("createAdminAccount")]
        [HttpPost]
        public async Task<IActionResult> CreateAdminAccount([FromForm] RegisterModel model)
        {
            var user = await _userService.AddAdminAccountAsync(model);
            if (user == null)
            {
                return new ObjectResult(new { code = 401, message = "Email exists" });
            }
            return new ObjectResult(new { code = 200, data = user });
        }

        [Authorize(Roles = RoleHelper.SuperAdmin)]
        [Route("createShipperAccount")]
        [HttpPost]
        public async Task<IActionResult> CreateShipperAccount([FromForm] RegisterModel model)
        {
            var user = await _userService.AddShipperAccountAsync(model);
            if (user == null)
            {
                return new ObjectResult(new { code = 401, message = "Email exists" });
            }
            return new ObjectResult(new { code = 200, data = user });
        }

        [Route("changePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordModel model)
        {
            var userId = (HttpContext.Items["User"] as UserDTO).Id;
            var result = await _userService.ChangePasswordAsync(userId, model);
            if (!result)
                return new ObjectResult(new { code = "401", message = "Error" });

            return new ObjectResult(new { code = "200", message = "Success" });
        }

        [AllowAnonymous]
        [Route("resetPassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordModel model)
        {
            var result = await _userService.ResetNewPasswordAsync(model);
            if (!result)
                return new ObjectResult(new { code = "401", message = "Error" });

            return new ObjectResult(new { code = "200", message = "Success" });
        }

        [AllowAnonymous]
        [Route("forgetPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgetPassword([FromForm] string email)
        {
            var result = await _userService.ForgetPasswordAsync(email);
            if (!result)
                return new ObjectResult(new { code = "401", message = "Error" });

            return new ObjectResult(new { code = "200", message = "Success" });
        }

        [Route("uploadImage")]
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm]IFormFile image)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _userService.UploadImageAsync(image, user.Id);
            if (!result)
                return new ObjectResult(new { code = "401", message = "Error" });

            return new ObjectResult(new { code = "200", message = "Success" });
        }

        [Route("update")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserInfo([FromForm] UpdateUserModel model)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            if (user == null)
                return new UnauthorizedObjectResult(new { code = "401", message = "Error" });

            model.Id = user.Id;
            var result = await _userService.UpdateInfoAsync(model);
            if (result == null)
                return new ObjectResult(new { code = "401", message = "Error" });

            return new ObjectResult(new { code = "200", data = result });
        }


        [Authorize(Roles = RoleHelper.Shipper)]
        [Route("getCommonShipperInfo")]
        [HttpGet]
        public async Task<IActionResult> GetCommonShipperInfo()
        {
            var user = HttpContext.Items["User"] as UserDTO;
            if (user == null)
            {
                return new ObjectResult(new { code = 401, message = "Fail" });
            }
            var rs = await _userService.GetCommonShipperInfoAsync(user.Id);
            return new ObjectResult(new { code = 200, data = rs });
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("getCommonAdminInfo")]
        [HttpGet]
        public async Task<IActionResult> GetCommonAdminInfo()
        {
            var user = HttpContext.Items["User"] as UserDTO;
            if (user == null)
            {
                return new ObjectResult(new { code = 401, message = "Fail" });
            }
            var rs = await _userService.GetCommonAdminInfoAsync();
            return new ObjectResult(new { code = 200, data = rs });
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("lockUser/{userId}")]
        [HttpGet]
        public async Task<IActionResult> LockUserAsync(int userId)
        {
            var rs = await _userService.LockUserAsync(userId);
            if(rs == false)
            {
                return new ObjectResult(new { code = 401, message = "Fail" });
            }
            return new ObjectResult(new { code = 200, message = "Success" });
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("unlockUser/{userId}")]
        [HttpGet]
        public async Task<IActionResult> UnlockUserAsync(int userId)
        {
            var rs = await _userService.UnlockUserAsync(userId);
            if (rs == false)
            {
                return new ObjectResult(new { code = 401, message = "Fail" });
            }
            return new ObjectResult(new { code = 200, message = "Success" });
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("getLockedAccount")]
        [HttpGet]
        public async Task<IActionResult> GetLockedAccountAsync()
        {
            var user = HttpContext.Items["User"] as UserDTO;
            if(user == null)
            {
                return new ObjectResult(new { code = 401 });
            }
            if(user.RoleName == RoleHelper.Admin)
            {
                var rs = await _userService.GetLockedUserAccountAsync();
                return new ObjectResult(new { code = 200, data = rs });
            }
            else
            {
                var rs = await _userService.GetAllLockedAccountAsync();
                return new ObjectResult(new { code = 200, data = rs });
            }
        }
    }
}
