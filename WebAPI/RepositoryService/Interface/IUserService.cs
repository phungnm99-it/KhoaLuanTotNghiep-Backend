using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ModelDTO;

namespace WebAPI.RepositoryService.Interface
{
    public interface IUserService
    {
        UserDTO Authenticate(string username, string password);
        IEnumerable<UserDTO> GetAll();
        UserDTO GetById(int id);
    }
}
