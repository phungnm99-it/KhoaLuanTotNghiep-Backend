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
            var checkEmailExist = await _unitOfWork.UserRepository.FindByCondition(user => user.Email == model.Email || user.Username == model.Username)
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
            
            user.RoleId = Helper.Role.AdminRoleId;
            user.CreatedDate = DateTime.Now;
            user.IsDeleted = false;
            user.IsDisable = false;

            _unitOfWork.UserRepository.CreateUser(user);
            await _unitOfWork.SaveAsync();
            var createdUser = await _unitOfWork.UserRepository.FindByCondition(
                user => user.Email == model.Email).FirstOrDefaultAsync();
            createdUser = await _unitOfWork.UserRepository.GetUserByIdAsync(createdUser.Id);
            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task<UserDTO> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            var user = await _unitOfWork.UserRepository
                .FindByCondition(user => user.Username == username)
                .FirstOrDefaultAsync();

            // return null if user not found
            if (user == null)
                return null;

            if (user.Password != _hash.GetHashPassword(password))
                return null;

            user = await _unitOfWork.UserRepository.GetUserByIdAsync(user.Id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> AuthenticateGoogleAsync(Payload payload)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(
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
                userGoogle.RoleId = Helper.Role.UserRoleId;
                userGoogle.IsDeleted = false;
                userGoogle.IsDisable = false;

                _unitOfWork.UserRepository.CreateUser(userGoogle);
                await _unitOfWork.SaveAsync();
                var createdUser = await _unitOfWork.UserRepository.FindByCondition(
                    user => user.Email == payload.Email).FirstOrDefaultAsync();
                createdUser = await _unitOfWork.UserRepository.GetUserByIdAsync(createdUser.Id);
                return _mapper.Map<UserDTO>(createdUser);
            }
            user = await _unitOfWork.UserRepository.GetUserByIdAsync(user.Id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(
                user => user.Id == userId).FirstAsync();
            if (user == null) return false;
            user.Password = _hash.GetHashPassword(newPassword);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> ForgetPasswordAsync(string email)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(user => user.Email == email).FirstOrDefaultAsync();
            if (user == null || user.IsDeleted == true || user.IsDisable == null)
                return false;

            string hashId = _hash.GetHashResetPassword(user.Id);
            MailRequest request = new MailRequest();
            request.ToEmail = email;
            request.Subject = "[PT Store] Reset mật khẩu";
            request.Body = "Truy cập đường link bên dưới để reset mật khẩu <br/>";
            request.Body += "https://localhost/resetPassword/" + hashId;
            await _mailService.SendEmailAsync(request);

            return true;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUserAsync()
        {
            List<UserDTO> list = new List<UserDTO>();
            var users = await _unitOfWork.UserRepository.GetAllUsersAsync();
            foreach(var user in users)
            {
                list.Add(_mapper.Map<UserDTO>(user));
            }
            return list;
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> RegisterAsync(RegisterModel model)
        {
            var checkEmailExist = await _unitOfWork.UserRepository.FindByCondition(user => user.Email == model.Email || user.Username == model.Username)
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
            user.RoleId = Helper.Role.UserRoleId;
            user.CreatedDate = DateTime.Now;
            user.IsGoogleLogin = false;
            user.IsDeleted = false;
            user.IsDisable = false;

            _unitOfWork.UserRepository.CreateUser(user);
            await _unitOfWork.SaveAsync();
            var createdUser = await _unitOfWork.UserRepository.FindByCondition(
                user => user.Email == model.Email).FirstOrDefaultAsync();
            createdUser = await _unitOfWork.UserRepository.GetUserByIdAsync(createdUser.Id);
            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task<bool> ResetNewPasswordAsync(ResetPasswordModel model)
        {
            var users = await _unitOfWork.UserRepository.GetAllUsersAsync();
            bool flag = false;
            foreach (var user in users)
            {
                if(_hash.GetHashResetPassword(user.Id) == model.HashId)
                {
                    flag = true;
                    user.Password = _hash.GetHashPassword(model.NewPassword);
                    _unitOfWork.UserRepository.UpdateUser(user);
                    await _unitOfWork.SaveAsync();
                    break;
                }
            }
            return flag;
        }

        public async Task<bool> UploadImageAsync(IFormFile image, int userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.FindByCondition(user => user.Id == userId).FirstOrDefaultAsync();
                if (image != null && image.Length != 0)
                {
                    string folder = "user/";
                    ImageUploadResult result = await _uploadImage.UploadImage(image, user.Email, folder) as ImageUploadResult;
                    user.ImageUrl = result.Url.ToString();
                }
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
