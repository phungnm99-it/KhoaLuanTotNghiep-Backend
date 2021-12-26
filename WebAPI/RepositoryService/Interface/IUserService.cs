using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.ModelDTO;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace WebAPI.RepositoryService.Interface
{
    public interface IUserService
    {
        Task<UserDTO> AuthenticateAsync(string username, string password);
        Task<UserDTO> AuthenticateGoogleAsync(Payload payload);
        Task<UserDTO> RegisterAsync(RegisterModel model);
        Task<IEnumerable<UserDTO>> GetAllUserAsync();
        Task<UserDTO> GetByIdAsync(int id);
        Task<UserDTO> AddAdminAccountAsync(RegisterModel model);
        Task<UserDTO> AddShipperAccountAsync(RegisterModel model);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordModel model);
        Task<bool> ResetNewPasswordAsync(ResetPasswordModel model);
        Task<bool> ForgetPasswordAsync(string email);
        public Task<bool> UploadImageAsync(IFormFile image, int userId);

        Task<UserDTO> AuthenticateAdminAsync(string username, string password);

        Task<UserDTO> AuthenticateShipperAsync(string username, string password);

        Task<List<ReviewDTO>> GetAllOwnReviewsAsync(int userId);

        Task<UserDTO> UpdateInfoAsync(UpdateUserModel model);

        Task<CommonShipperInfo> GetCommonShipperInfoAsync(int shipperId);

        Task<CommonAdminInfo> GetCommonAdminInfoAsync();

        Task<bool> LockUserAsync(int userId);

        Task<bool> UnlockUserAsync(int userId);
        Task<bool> LockShipperAsync(int userId);
        Task<bool> UnlockShipperAsync(int userId);
        Task<bool> LockAdminAsync(int userId);
        Task<bool> UnlockAdminAsync(int userId);
    }
}
