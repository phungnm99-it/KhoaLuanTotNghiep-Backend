using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helper;
using WebAPI.ModelDTO;
using WebAPI.RepositoryService.Interface;

namespace WebAPI.RepositoryService.Service
{
    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<UserDTO> _users = new List<UserDTO>
        {
            new UserDTO { Id = 1, FirstName = "Admin", LastName = "User", Username = "admin", Password = "admin", Role = Role.Admin },
            new UserDTO { Id = 2, FirstName = "Normal", LastName = "User", Username = "user", Password = "user", Role = Role.User }
        };

        

        public UserDTO Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            return user;
        }

        public IEnumerable<UserDTO> GetAll()
        {
            return _users;
        }

        public UserDTO GetById(int id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);
            return user;
        }
    }
}
