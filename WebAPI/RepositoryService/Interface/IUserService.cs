﻿using System;
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

        
    }
}
