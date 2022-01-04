using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Helper;
using WebAPI.Models;
using WebAPI.ModelDTO;
using WebAPI.RepositoryService.Interface;
using WebAPI.UnitOfWorks;
using WebAPI.UploadImageUtils;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using Microsoft.AspNetCore.Http;
using WebAPI.MailKit;

namespace WebAPI.RepositoryService.Service
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;
        private IUploadImage _uploadImage;
        private IMapper _mapper;
        private ICustomHash _hash;
        private IMailService _mailService;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IUploadImage uploadImageUtils,
            ICustomHash hashPassword, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadImage = uploadImageUtils;
            _hash = hashPassword;
            _mailService = mailService;
        }

        public async Task<UserDTO> AddAdminAccountAsync(RegisterModel model)
        {
            var checkEmailExist = await _unitOfWork.Users.FindByCondition(user => user.Email == model.Email || user.Username == model.Username || user.PhoneNumber == model.PhoneNumber)
                .FirstOrDefaultAsync();
            if (checkEmailExist != null)
                return null;
            User user = new User();
            user.Username = model.Username;
            user.Email = model.Email;
            user.Gender = model.Gender;
            user.Password = _hash.GetHashPassword(model.Password);
            user.FullName = model.FullName;
            user.Birthday = model.Birthday;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;
            user.ImageUrl = "";
            user.IsEmailConfirmed = false;
            
            user.RoleId = Helper.RoleHelper.AdminRoleId;
            user.CreatedDate = DateTime.Now;
            user.IsDeleted = false;
            user.IsDisable = false;

            _unitOfWork.Users.CreateUser(user);
            await _unitOfWork.SaveAsync();
            var createdUser = await _unitOfWork.Users.FindByCondition(
                user => user.Email == model.Email).FirstOrDefaultAsync();
            createdUser = await _unitOfWork.Users.GetUserByIdAsync(createdUser.Id);
            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task<UserDTO> AddShipperAccountAsync(RegisterModel model)
        {
            var checkEmailExist = await _unitOfWork.Users.FindByCondition(user => user.Email == model.Email || user.Username == model.Username || user.PhoneNumber == model.PhoneNumber)
                .FirstOrDefaultAsync();
            if (checkEmailExist != null)
                return null;
            User user = new User();
            user.Username = model.Username;
            user.Email = model.Email;
            user.Gender = model.Gender;
            user.Password = _hash.GetHashPassword(model.Password);
            user.FullName = model.FullName;
            user.Birthday = model.Birthday;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;
            user.ImageUrl = "";
            user.IsEmailConfirmed = false;

            user.RoleId = Helper.RoleHelper.ShipperRoleId;
            user.CreatedDate = DateTime.Now;
            user.IsDeleted = false;
            user.IsDisable = false;

            _unitOfWork.Users.CreateUser(user);
            await _unitOfWork.SaveAsync();
            var createdUser = await _unitOfWork.Users.FindByCondition(
                user => user.Email == model.Email).FirstOrDefaultAsync();
            createdUser = await _unitOfWork.Users.GetUserByIdAsync(createdUser.Id);
            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task<UserDTO> AuthenticateAdminAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            var user = await _unitOfWork.Users
                .FindByCondition(user => user.Username == username)
                .FirstOrDefaultAsync();

            // return null if user not found
            if (user == null)
                return null;

            if (user.Password != _hash.GetHashPassword(password))
                return null;
            if (user.RoleId != RoleHelper.AdminRoleId && user.RoleId != RoleHelper.SuperAdminRoleId)
                return null;
            user = await _unitOfWork.Users.GetUserByIdAsync(user.Id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> AuthenticateShipperAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            var user = await _unitOfWork.Users
                .FindByCondition(user => user.Username == username)
                .FirstOrDefaultAsync();

            // return null if user not found
            if (user == null)
                return null;

            if (user.Password != _hash.GetHashPassword(password))
                return null;
            if (user.RoleId != RoleHelper.ShipperRoleId)
                return null;
            user = await _unitOfWork.Users.GetUserByIdAsync(user.Id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            var user = await _unitOfWork.Users
                .FindByCondition(user => user.Username == username)
                .FirstOrDefaultAsync();

            // return null if user not found
            if (user == null)
                return null;

            if (user.Password != _hash.GetHashPassword(password))
                return null;

            if (user.IsDeleted == true || user.IsDisable == true)
                return null;

            user = await _unitOfWork.Users.GetUserByIdAsync(user.Id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> AuthenticateGoogleAsync(Payload payload)
        {
            var user = await _unitOfWork.Users.FindByCondition(
                user => user.Email == payload.Email).FirstOrDefaultAsync();

            if(user == null)
            {
                User userGoogle = new User();
                userGoogle.FullName = payload.Name;
                userGoogle.Gender = "";
                userGoogle.IsEmailConfirmed = true;
                userGoogle.Email = payload.Email;
                userGoogle.Birthday = DateTime.Now;
                userGoogle.ImageUrl = payload.Picture;
                userGoogle.IsGoogleLogin = true;
                userGoogle.CreatedDate = DateTime.Now;
                userGoogle.RoleId = Helper.RoleHelper.UserRoleId;
                userGoogle.IsDeleted = false;
                userGoogle.IsDisable = false;

                _unitOfWork.Users.CreateUser(userGoogle);
                await _unitOfWork.SaveAsync();
                var createdUser = await _unitOfWork.Users.FindByCondition(
                    user => user.Email == payload.Email).FirstOrDefaultAsync();
                createdUser = await _unitOfWork.Users.GetUserByIdAsync(createdUser.Id);
                return _mapper.Map<UserDTO>(createdUser);
            }
            user = await _unitOfWork.Users.GetUserByIdAsync(user.Id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordModel model)
        {
            try
            {
                var user = await _unitOfWork.Users.FindByCondition(
                user => user.Id == userId).FirstAsync();
                if (user == null) return false;
                if (!user.Password.Equals(_hash.GetHashPassword(model.OldPassword)))
                    return false;
                user.Password = _hash.GetHashPassword(model.NewPassword);
                _unitOfWork.Users.UpdateUser(user);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ForgetPasswordAsync(string email)
        {
            var user = await _unitOfWork.Users.FindByCondition(user => user.Email == email).FirstOrDefaultAsync();
            if (user == null || user.IsDeleted == true || user.IsDisable == true)
                return false;

            string hashId = _hash.GetHashResetPassword(user.Id);
            MailRequest request = new MailRequest();
            request.ToEmail = email;
            request.Subject = "[PT Store] Reset mật khẩu";
            request.Body = "Truy cập đường link bên dưới để reset mật khẩu <br/>";
            request.Body += "http://localhost:3000/resetPassword/" + hashId;
            await _mailService.SendEmailAsync(request);

            return true;
        }

        public async Task<List<ReviewDTO>> GetAllOwnReviewsAsync(int userId)
        {
            try
            {
                var reviews = await _unitOfWork.Reviews.GetAllOwnReviewsAsync(userId);
                List<ReviewDTO> list = new List<ReviewDTO>();
                foreach(var item in reviews)
                {
                    list.Add(_mapper.Map<ReviewDTO>(item));
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllUserAsync()
        {
            List<UserDTO> list = new List<UserDTO>();
            var users = await _unitOfWork.Users.GetAllUsersAsync();
            foreach(var user in users)
            {
                if (user.RoleId == RoleHelper.UserRoleId && user.IsDeleted == false)
                    list.Add(_mapper.Map<UserDTO>(user));
            }
            return list;
        }

        public async Task<IEnumerable<UserDTO>> GetAllAdminAsync()
        {
            List<UserDTO> list = new List<UserDTO>();
            var users = await _unitOfWork.Users.GetAllUsersAsync();
            foreach (var user in users)
            {
                if(user.RoleId == RoleHelper.AdminRoleId && user.IsDeleted == false)
                    list.Add(_mapper.Map<UserDTO>(user));
            }
            return list;
        }

        public async Task<IEnumerable<UserDTO>> GetAllShipperAsync()
        {
            List<UserDTO> list = new List<UserDTO>();
            var users = await _unitOfWork.Users.GetAllUsersAsync();
            foreach (var user in users)
            {
                if (user.RoleId == RoleHelper.ShipperRoleId && user.IsDeleted == false)
                    list.Add(_mapper.Map<UserDTO>(user));
            }
            return list;
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> RegisterAsync(RegisterModel model)
        {
            var checkEmailExist = await _unitOfWork.Users.FindByCondition(user => user.PhoneNumber == model.PhoneNumber || user.Email == model.Email || user.Username == model.Username)
                .FirstOrDefaultAsync();
            if (checkEmailExist != null)
                return null;
            User user = new User();
            user.Username = model.Username;
            user.Gender = model.Gender;
            user.Password = _hash.GetHashPassword(model.Password);
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Birthday = model.Birthday;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;
            user.ImageUrl = "";
            user.IsEmailConfirmed = false;
            user.RoleId = Helper.RoleHelper.UserRoleId;
            user.CreatedDate = DateTime.Now;
            user.IsGoogleLogin = false;
            user.IsDeleted = false;
            user.IsDisable = false;

            _unitOfWork.Users.CreateUser(user);
            await _unitOfWork.SaveAsync();
            var createdUser = await _unitOfWork.Users.FindByCondition(
                user => user.Email == model.Email).FirstOrDefaultAsync();
            createdUser = await _unitOfWork.Users.GetUserByIdAsync(createdUser.Id);
            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task<bool> ResetNewPasswordAsync(ResetPasswordModel model)
        {
            var users = await _unitOfWork.Users.GetAllUsersAsync();
            bool flag = false;
            foreach (var user in users)
            {
                if(_hash.GetHashResetPassword(user.Id) == model.HashId)
                {
                    flag = true;
                    user.Password = _hash.GetHashPassword(model.NewPassword);
                    _unitOfWork.Users.UpdateUser(user);
                    await _unitOfWork.SaveAsync();
                    break;
                }
            }
            return flag;
        }

        public async Task<UserDTO> UpdateInfoAsync(UpdateUserModel model)
        {
            try
            {
                var currentUser = await _unitOfWork.Users.GetUserByIdAsync(model.Id);
                if (currentUser == null)
                    return null;
               if(!string.IsNullOrEmpty(model.Address))
                {
                    currentUser.Address = model.Address;
                }
               if(!string.IsNullOrEmpty(model.FullName))
                {
                    currentUser.FullName = model.FullName;
                }
               if(!string.IsNullOrEmpty(model.Gender))
                {
                    currentUser.Gender = model.Gender;
                }

               if(model.Birthday < DateTime.Now && model.Birthday > (new DateTime(1900,1,1)))
                {
                    currentUser.Birthday = model.Birthday;
                }

                _unitOfWork.Users.UpdateUser(currentUser);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<UserDTO>(currentUser);
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<bool> UploadImageAsync(IFormFile image, int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.FindByCondition(user => user.Id == userId).FirstOrDefaultAsync();
                if (image != null && image.Length != 0)
                {
                    string folder = "user/" + user.Id.ToString() + "/";
                    ImageUploadResult result = await _uploadImage.UploadImage(image, DateTime.Now.Ticks.ToString(), folder) as ImageUploadResult;
                    user.ImageUrl = result.Url.ToString();
                    _unitOfWork.Users.UpdateUser(user);
                    await _unitOfWork.SaveAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public async Task<CommonShipperInfo> GetCommonShipperInfoAsync(int shipperId)
        {
            var user = await _unitOfWork.Users.FindByCondition(user => user.Id == shipperId).FirstOrDefaultAsync();
            CommonShipperInfo rs = new CommonShipperInfo();
            rs.WorkingDate = (DateTime.Now - user.CreatedDate).Days;

            var orders = await _unitOfWork.Orders.FindByCondition(od => od.ShipperId == shipperId && od.IsCompleted == true).ToListAsync();
            rs.DeliveredOrder = orders.Count();

            var orders2 = await _unitOfWork.Orders.FindByCondition(od => od.ShipperId == shipperId && od.IsCompleted == false
            && od.Status != "Đã huỷ").ToListAsync();
            rs.DeliveringOrder = orders2.Count();

            var order3 = await _unitOfWork.Orders.FindByCondition(od => od.IsCompleted == false && od.Status != "Đã huỷ"
            && od.ShipperId != shipperId).ToListAsync();
            rs.TotalOrder = order3.Count();

            return rs;
        }

        public async Task<CommonAdminInfo> GetCommonAdminInfoAsync()
        {
            CommonAdminInfo rs = new CommonAdminInfo();
            rs.TotalAccount = _unitOfWork.Users.FindAll().Count();
            rs.TotalProduct = _unitOfWork.Products.FindByCondition(pr => pr.IsDeleted == false).Count();
            rs.TotalOrder = _unitOfWork.Orders.FindByCondition(od => od.Status != "Đã huỷ").Count();
            rs.TotalBrand = _unitOfWork.Brands.FindAll().Count();
            return rs;
        }

        public async Task<bool> LockUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user == null)
                return false;
            if (user.IsDeleted == true)
                return false;

            if (user.IsDisable == true)
                return false;

            if (user.RoleId == RoleHelper.SuperAdminRoleId)
                return false;

            user.IsDisable = true;
            _unitOfWork.Users.UpdateUser(user);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UnlockUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user == null)
                return false;
            if (user.IsDeleted == true)
                return false;

            if (user.IsDisable == false)
                return false;


            user.IsDisable = false;
            _unitOfWork.Users.UpdateUser(user);
            await _unitOfWork.SaveAsync();
            return true;
        }


        public async Task<IEnumerable<UserDTO>> GetAllLockedAccountAsync()
        {
            List<UserDTO> list = new List<UserDTO>();
            var users = await _unitOfWork.Users.GetAllUsersAsync();
            foreach (var user in users)
            {
                if (user.IsDisable == true && user.IsDeleted == false)
                    list.Add(_mapper.Map<UserDTO>(user));
            }
            return list;
        }

        public async Task<IEnumerable<UserDTO>> GetLockedUserAccountAsync()
        {
            List<UserDTO> list = new List<UserDTO>();
            var users = await _unitOfWork.Users.GetAllUsersAsync();
            foreach (var user in users)
            {
                if (user.IsDisable == true && user.IsDeleted == false && user.RoleId == RoleHelper.UserRoleId)
                    list.Add(_mapper.Map<UserDTO>(user));
            }
            return list;
        }
    }
}
