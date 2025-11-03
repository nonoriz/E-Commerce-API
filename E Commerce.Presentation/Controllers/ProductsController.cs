using E_Commerce.Services_Abstraction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await productService.GetProductByIdAsync(id);

            return Ok(product);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetProductsByType()
        {
            var products = await productService.GetAllTypesAsync();
            return Ok(products);
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetProductsByBrand()
        {
            var products = await productService.GetAllBrandsAsync();
            return Ok(products);
        }
    }
}
