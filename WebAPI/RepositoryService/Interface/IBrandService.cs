using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.ModelDTO;

namespace WebAPI.RepositoryService.Interface
{
    public interface IBrandService
    {
        public Task<BrandDTO> GetBrandByIdAsync(int brandId);

        public Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();

        public Task<BrandDTO> CreateBrandAsync(BrandModel brandModel);

        public Task<BrandDTO> UpdateBrandAsync(BrandUpdateModel brandModel);

        public Task<bool> DeleteBrandAsync(int brandId);
        public Task<bool> RestoreBrandAsync(int brandId);

        public Task<IEnumerable<BrandDTO>> GetActiveBrandAsync();

        public Task<List<BrandDTO>> SearchBrand(string name);
    }
}
