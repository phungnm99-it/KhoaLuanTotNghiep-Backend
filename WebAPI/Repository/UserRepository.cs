using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Model;
using WebAPI.Repository.Interface;

namespace WebAPI.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(PTStoreContext context) : base(context) { }

        public void CreateUser(User user)
        {
            Create(user);
        }

        public void DeleteUser(User user)
        {
            Delete(user);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await FindAll()
                .Include(user=> user.Role).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await RepositoryContext.Users
                .Include(user => user.Role)
                .Where(user => user.Id == userId)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserWithDetailsAsync(int userId)
        {
            return await RepositoryContext.Users
                .Include(user => user.Role)
                .Include(user => user.OrderUsers)
                .Include(user => user.Reviews)
                .Where(user => user.Id == userId)
                .FirstOrDefaultAsync();
        }

        public void UpdateUser(User user)
        {
            Update(user);
        }
    }
}
