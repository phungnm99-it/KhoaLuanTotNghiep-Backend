using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repository.Interface;

namespace WebAPI.Repository
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(PTStoreContext context) : base(context) { }
        public void CreateBrand(Brand brand)
        {
            Create(brand);
        }

        public void DeleteBrand(Brand brand)
        {
            Delete(brand);
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await FindAll().Include(brand => brand.Products).ToListAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int brandId)
        {
            return await FindByCondition(brand => brand.Id == brandId).FirstOrDefaultAsync();
        }

        public async Task<Brand> GetBrandWithDetailsAsync(int brandId)
        {
            return await FindByCondition(brand => brand.Id == brandId)
                .Include(brand => brand.Products).FirstOrDefaultAsync();
        }

        public void UpdateBrand(Brand brand)
        {
            Update(brand);
        }
    }
}
