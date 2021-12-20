using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Repository.Interface
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<IEnumerable<Brand>> GetAllBrandsAsync();
        Task<Brand> GetBrandByIdAsync(int brandId);
        Task<Brand> GetBrandWithDetailsAsync(int brandId);
        void CreateBrand(Brand brand);
        void UpdateBrand(Brand brand);
        //void DeleteBrand(Brand brand);
    }
}
