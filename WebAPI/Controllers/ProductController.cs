using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Helper;
using WebAPI.ModelDTO;
using WebAPI.RepositoryService.Interface;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            return new ObjectResult(new { code = 200, data = product });
        }

        [AllowAnonymous]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _service.GetAllSellingProductsAsync();
            return new ObjectResult(new { code = 200, data = products });
        }

        [Authorize(Roles = RoleHelper.Admins)]
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

        [Authorize(Roles = RoleHelper.Admins)]
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

        [Authorize(Roles = RoleHelper.Admins)]
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

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("productstock")]
        [HttpGet]
        public async Task<IActionResult> GetAllProductsWithStock()
        {
            var products = await _service.GetAllProductsWithStockAsync();
            return new ObjectResult(new { code = 200, data = products });
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("productstock/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetProductWithStock(int id)
        {
            var product = await _service.GetProductWithStockByIdAsync(id);
            return new ObjectResult(new { code = 200, data = product });
        }


        [Authorize(Roles = RoleHelper.Admins)]
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

        [AllowAnonymous]
        [Route("featureproduct")]
        [HttpGet]
        public async Task<IActionResult> GetFeatureProducts()
        {
            var products = await _service.GetAllFeatureProductsAsync();
            return new ObjectResult(new { code = 200, data = products });
        }

        [Authorize(Roles = RoleHelper.Admins)]
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

        [Authorize(Roles = RoleHelper.Admins)]
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

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("productprice")]
        [HttpGet]
        public async Task<IActionResult> GetAllProductsWithPrice()
        {
            var products = await _service.GetAllProductsWithPriceAsync();
            return new ObjectResult(new { code = 200, data = products });
        }

        [Authorize(Roles = RoleHelper.Admins)]
        [Route("productprice/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetProductWithPrice(int id)
        {
            var product = await _service.GetProductWithPriceByIdAsync(id);
            return new ObjectResult(new { code = 200, data = product });
        }

        [Authorize(Roles = RoleHelper.Admins)]
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

        [AllowAnonymous]
        [Route("sale")]
        [HttpGet]
        public async Task<IActionResult> GetAllSaleProduct()
        {
            var products = await _service.GetAllSaleProductsAsync();
            return new ObjectResult(new { code = 200, data = products });
        }

        [AllowAnonymous]
        [Route("brand/{brandname}")]
        [HttpGet]
        public async Task<IActionResult> FindAllProductsByBrandName(string brandName)
        {
            var products = await _service.FindProductsByBrandNameAsync(brandName);
            return new ObjectResult(new { code = 200, data = products });
        }

        [AllowAnonymous]
        [Route("name={productname}")]
        [HttpGet]
        public async Task<IActionResult> FindAllProductsByProductName(string productname)
        {
            var products = await _service.FindProductsByProductNameAsync(productname);
            return new ObjectResult(new { code = 200, data = products });
        }

        [Authorize(Roles = RoleHelper.SuperAdmin)]
        [Route("fix")]
        [HttpPost]
        public async Task<IActionResult> FixProduct([FromForm] IFormFile file)
        {
            var product = await _service.Modify(file);
            if (product == null)
            {
                return new ObjectResult(new { code = 401, message = "Error!" });
            }
            return new ObjectResult(new { code = 200, data = product });
        }

        [AllowAnonymous]
        [Route("similar/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetSimilarProducts(int id)
        {
            var products = await _service.GetSimilarProductsAsync(id);
            return new ObjectResult(new { code = 200, data = products });
        }

        [AllowAnonymous]
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> GetActiveProducts([FromBody] SortModel model)
        {
            var products = await _service.GetActiveProductsAsync(model);
            return new ObjectResult(new { code = 200, data = products });
        }

        [Route("checkIfBuy/{productId}")]
        [HttpGet]
        public async Task<IActionResult> CheckIfUserBuyProductAsync(int productId)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _service.CheckUserIdIfBuyProductIdAsync(user.Id, productId);
            return new ObjectResult(new { code = 200, data = result });
        }

        [Route("review/create")]
        [HttpPost]
        public async Task<IActionResult> CreateReviewAsync([FromForm] ReviewModel model)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            model.UserId = user.Id;
            var result = await _service.CreateReviewAsync(model);
            if (!result)
            {
                return new ObjectResult(new { code = 401, message = "Fail" });
            }
            return new ObjectResult(new { code = 200, message = "Success!" });
        }

        [Route("checkCanReview/{productId}")]
        [HttpGet]
        public async Task<IActionResult> CheckIfUserBuyButNotReviewAsync(int productId)
        {
            var user = HttpContext.Items["User"] as UserDTO;
            var result = await _service.CheckUserBuyProductButNotReviewAsync(user.Id, productId);
            if (!result)
            {
                return new ObjectResult(new { code = 401, message = "Fail" });
            }
            return new ObjectResult(new { code = 200, message = "Success!" });
        }

        [AllowAnonymous]
        [Route("review/{productId}")]
        [HttpGet]
        public async Task<IActionResult> GetReviewsByProductIdAsync(int productId)
        {
            var result = await _service.GetAllReviewsByProductIdAsync(productId);
            return new ObjectResult(new { code = 200, message = result });
        }

        [AllowAnonymous]
        [Route("bestSell")]
        [HttpGet]
        public async Task<IActionResult> GetBestSellProductsAsync()
        {
            var result = await _service.GetBestSellProductAsync();
            return new ObjectResult(new { code = 200, data = result });
        }

        [Authorize(Roles =RoleHelper.Admins)]
        [Route("review/getAll")]
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var result = await _service.GetAllReviewAsync();
            return new ObjectResult(new { code = 200, data = result });
        }

        [AllowAnonymous]
        [Route("filter")]
        [HttpPost]
        public async Task<IActionResult> Find(string brand, string priceFilter, string sortType, int page)
        {
            var result = await _service.SearchProductsByFilterAsync(brand, priceFilter, sortType, page);
            return new ObjectResult(new { code = 200, data = result.Item1, count = result.count });
        }

        [AllowAnonymous]
        [Route("sale/filter")]
        [HttpPost]
        public async Task<IActionResult> FindSale(string brand, string priceFilter, string sortType, int page)
        {
            var result = await _service.SearchSaleProductsByFilterAsync(brand, priceFilter, sortType, page);
            return new ObjectResult(new { code = 200, data = result.Item1, count = result.count });
        }
    }
}
