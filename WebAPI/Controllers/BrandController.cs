using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.RepositoryService.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : Controller
    {
        private IBrandService _brandService { get; set; }
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [Route("getAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _brandService.GetAllBrandsAsync();
            return new OkObjectResult(new { code = "200", data = list });
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromForm] BrandModel brandModel)
        {
            var brand = await _brandService.CreateBrandAsync(brandModel);
            if (brand == null)
                return new ObjectResult(new { code = 401, message = "BrandName exists!" });
            return new ObjectResult(new { code = 200, data = brand });
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            return new ObjectResult(new { code = 200, data = brand });
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> UpdateBrand([FromForm] BrandUpdateModel brandModel)
        {
            var brand = await _brandService.UpdateBrandAsync(brandModel);
            if (brand == null)
                return new ObjectResult(new { code = 401, message = "BrandName exists or BrandId not exists!" });
            return new ObjectResult(new { code = 200, data = brand });
        }

        [Route("delete")]
        [HttpPatch]
        public async Task<IActionResult> DeleteBrand([FromForm] int id)
        {
            var result = await _brandService.DeleteBrandAsync(id);
            if (result == false)
                return new ObjectResult(new { code = 401, message = "Delete failed!" });
            return new ObjectResult(new { code = 200, message = "Delete success!" });
        }

        [Route("restore")]
        [HttpPatch]
        public async Task<IActionResult> RestoreBrand([FromForm] int id)
        {
            var result = await _brandService.RestoreBrandAsync(id);
            if (result == false)
                return new ObjectResult(new { code = 401, message = "Restore failed!" });
            return new ObjectResult(new { code = 200, message = "Restore success!" });
        }
    }
}
