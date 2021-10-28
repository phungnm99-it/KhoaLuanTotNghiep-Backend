using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
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
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            return new ObjectResult(new { code = 200, data = product });
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _service.GetAllSellingProductsAsync();
            return new ObjectResult(new { code = 200, data = products });
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductModel productModel)
        {
            var product = await _service.CreateProductAsync(productModel);
            if(product==null)
            {
                return new ObjectResult(new { code = 401, message = "Error!" });
            }
            return new ObjectResult(new { code = 200, data = product });
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateModel productModel)
        {
            var product = await _service.UpdateProductAsync(productModel);
            if (product == null)
            {
                return new ObjectResult(new { code = 401, message = "Error!" });
            }
            return new ObjectResult(new { code = 200, data = product });
        }

        [Route("delete")]
        [HttpPatch]
        public async Task<IActionResult> DeleteProduct([FromForm] int id)
        {
            var product = await _service.DeleteProductAsync(id);
            if (!product)
            {
                return new ObjectResult(new { code = 401, message = "Wrong Id!" });
            }
            return new ObjectResult(new { code = 200, message = "Success!" });
        }

        [Route("productstock")]
        [HttpGet]
        public async Task<IActionResult> GetAllProductsWithStock()
        {
            var products = await _service.GetAllProductsWithStockAsync();
            return new ObjectResult(new { code = 200, data = products });
        }

        [Route("productstock/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetProductWithStock(int id)
        {
            var product = await _service.GetProductWithStockByIdAsync(id);
            return new ObjectResult(new { code = 200, data = product });
        }


        [Route("productstock/update")]
        [HttpPut]
        public async Task<IActionResult> UpdateProductWithStock([FromForm] ProductStockManager productStock)
        {
            var product = await _service.UpdateProductWithStockAsync(productStock);
            if(product == null)
            {
                return new ObjectResult(new { code = 401, message = "Wrong Id!" });
            }
            return new ObjectResult(new { code = 200, data = product });
        }


        [Route("featureproduct")]
        [HttpGet]
        public async Task<IActionResult> GetFeatureProducts()
        {
            var products = await _service.GetAllFeatureProductsAsync();
            return new ObjectResult(new { code = 200, data = products });
        }

        [Route("featureproduct/add")]
        [HttpPut]
        public async Task<IActionResult> AddFeatureProductById([FromForm]int id)
        {
            var product = await _service.AddFeatureProductByIdAsync(id);
            if (!product)
            {
                return new ObjectResult(new { code = 401, message = "Wrong Id!" });
            }
            return new ObjectResult(new { code = 200, message = "Success!" });
        }

        [Route("featureproduct/remove")]
        [HttpPut]
        public async Task<IActionResult> RemoveFeatureProductById([FromForm] int id)
        {
            var product = await _service.RemoveFeatureProductByIdAsync(id);
            if (!product)
            {
                return new ObjectResult(new { code = 401, message = "Wrong Id!" });
            }
            return new ObjectResult(new { code = 200, message = "Success!" });
        }


        [Route("productprice")]
        [HttpGet]
        public async Task<IActionResult> GetAllProductsWithPrice()
        {
            var products = await _service.GetAllProductsWithPriceAsync();
            return new ObjectResult(new { code = 200, data = products });
        }

        [Route("productprice/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetProductWithPrice(int id)
        {
            var product = await _service.GetProductWithPriceByIdAsync(id);
            return new ObjectResult(new { code = 200, data = product });
        }


        [Route("productprice/update")]
        [HttpPut]
        public async Task<IActionResult> UpdateProductWithPrice([FromForm] ProductPriceManager productPrice)
        {
            var product = await _service.UpdateProductWithPriceAsync(productPrice);
            if (product == null)
            {
                return new ObjectResult(new { code = 401, message = "Wrong Id!" });
            }
            return new ObjectResult(new { code = 200, data = product });
        }

        [Route("saleproduct")]
        [HttpGet]
        public async Task<IActionResult> GetAllSaleProduct()
        {
            var products = await _service.GetAllSaleProductsAsync();
            return new ObjectResult(new { code = 200, data = products });
        }

        [Route("brand/{brandname}")]
        [HttpGet]
        public async Task<IActionResult> FindAllProductsByBrandName(string brandName)
        {
            var products = await _service.FindProductsByBrandName(brandName);
            return new ObjectResult(new { code = 200, data = products });
        }

        [Route("name={productname}")]
        [HttpGet]
        public async Task<IActionResult> FindAllProductsByProductName(string productname)
        {
            var products = await _service.FindProductsByProductName(productname);
            return new ObjectResult(new { code = 200, data = products });
        }
        //[Route("fix")]
        //[HttpPost]
        //public async Task<IActionResult> FixProduct([FromForm] IFormFile file)
        //{
        //    var product = await _service.Modify(file);
        //    if (product == null)
        //    {
        //        return new ObjectResult(new { code = 401, message = "Error!" });
        //    }
        //    return new ObjectResult(new { code = 200, data = product });
        //}


    }
}
