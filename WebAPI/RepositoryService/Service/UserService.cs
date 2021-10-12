using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Helper;
using WebAPI.Model;
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
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IUploadImage uploadImageUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadImage = uploadImageUtils;
        }

        public async Task<UserDTO> AddAdminAccount(RegisterModel model)
        {
            var checkEmailExist = await _unitOfWork.UserRepository.FindByCondition(user => user.Email == model.Email)
                .FirstOrDefaultAsync();
            if (checkEmailExist != null)
                return null;
            User user = new User();
            user = _mapper.Map<User>(model);
            user.IsEmailConfirmed = false;
            if (model.Image != null && model.Image.Length != 0)
            {
                ImageUploadResult result = await _uploadImage.UploadImage(model.Image, model.Email) as ImageUploadResult;
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
            var user = await _unitOfWork.UserRepository
                .FindByCondition(user => user.Username == username && user.Password == password)
                .FirstOrDefaultAsync();

            // return null if user not found
            if (user == null)
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
                userGoogle.Username = payload.Email;
                userGoogle.FullName = payload.Name;
                userGoogle.Gender = "";
                userGoogle.IsEmailConfirmed = true;
                userGoogle.Email = payload.Email;
                userGoogle.Birthday = DateTime.Now;
                userGoogle.ImageUrl = payload.Picture;
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

        public async Task ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await _unitOfWork.UserRepository.FindByCondition(
                user => user.Id == userId).FirstAsync();
            user.Password = newPassword;
            await _unitOfWork.SaveAsync();
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
            user = _mapper.Map<User>(model);
            user.IsEmailConfirmed = false;
            if (model.Image != null && model.Image.Length != 0)
            {
                ImageUploadResult result = await _uploadImage.UploadImage(model.Image, model.Email) as ImageUploadResult;
                user.ImageUrl = result.Url.ToString();
            }
            user.RoleId = Helper.Role.UserRoleId;
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


    }
}
