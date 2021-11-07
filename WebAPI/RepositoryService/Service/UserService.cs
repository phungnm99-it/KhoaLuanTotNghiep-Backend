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

namespace WebAPI.RepositoryService.Service
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;
        private IUploadImage _uploadImage;
        private IMapper _mapper;
        private ICustomHash _hashPassword;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IUploadImage uploadImageUtils,
            ICustomHash hashPassword)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadImage = uploadImageUtils;
            _hashPassword = hashPassword;
        }

        public async Task<UserDTO> AddAdminAccount(RegisterModel model)
        {
            var checkEmailExist = await _unitOfWork.UserRepository.FindByCondition(user => user.Email == model.Email)
                .FirstOrDefaultAsync();
            if (checkEmailExist != null)
                return null;
            User user = new User();
            user.Username = model.Username;
            user.Password = _hashPassword.GetHashPassword(model.Password);
            user.IsEmailConfirmed = false;
            if (model.Image != null && model.Image.Length != 0)
            {
                string folder = "user/";
                ImageUploadResult result = await _uploadImage.UploadImage(model.Image, model.Email, folder) as ImageUploadResult;
                user.ImageUrl = result.Url.ToString();
            }
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

            if (user.Password != _hashPassword.GetHashPassword(password))
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
            user.Password = _hashPassword.GetHashPassword(newPassword);
            await _unitOfWork.SaveAsync();
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
            var checkEmailExist = await _unitOfWork.UserRepository.FindByCondition(user => user.Email == model.Email)
                .FirstOrDefaultAsync();
            if (checkEmailExist != null)
                return null;
            User user = new User();
            user.Username = model.Username;
            user.Password = _hashPassword.GetHashPassword(model.Password);
            user.IsEmailConfirmed = false;
            if (model.Image != null && model.Image.Length != 0)
            {
                string folder = "user/";
                ImageUploadResult result = await _uploadImage.UploadImage(model.Image, model.Email, folder) as ImageUploadResult;
                user.ImageUrl = result.Url.ToString();
            }
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

        public async Task<bool> ResetNewPassword(ResetPasswordModel model)
        {
            var users = await _unitOfWork.UserRepository.GetAllUsersAsync();
            bool flag = false;
            foreach (var user in users)
            {
                if(_hashPassword.GetHashResetPassword(user.Id) == model.HashId)
                {
                    flag = true;
                    user.Password = _hashPassword.GetHashPassword(model.NewPassword);
                    _unitOfWork.UserRepository.UpdateUser(user);
                    await _unitOfWork.SaveAsync();
                    break;
                }
            }
            return flag;
        }
    }
}
