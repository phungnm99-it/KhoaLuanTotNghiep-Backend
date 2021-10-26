using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Repository.Interface;

namespace WebAPI.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; set; }
        IUserRepository UserRepository { get; set; }

        IBrandRepository BrandRepository { get; set; }
        Task SaveAsync();
    }
}
