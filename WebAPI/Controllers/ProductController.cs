using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ModelDTO;
using WebAPI.RepositoryService.Interface;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }
        [Route("{productId}")]
        [HttpGet]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _service.GetProductByIdAsync(productId);
            return new ObjectResult(new { code = 200, data = product });
        }

        [Route("all")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _service.GetAllProductAsync();
            return new ObjectResult(new { code = 200, data = products });
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDTO product)
        {
            var products = await _service.GetAllProductAsync();
            return new ObjectResult(new { code = 200, data = products });
        }
    }
}
