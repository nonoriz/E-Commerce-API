using E_Commerce.Services_Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
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
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery]ProductQueryParams queryParams)
        {
            var products = await productService.GetAllProductsAsync(queryParams);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {

            var product = await productService.GetProductByIdAsync(id);

            return Ok(product);
        }

        [HttpGet("types")]
        public async Task<ActionResult> GetProductsByType()
        {
            var products = await productService.GetAllTypesAsync();
            return Ok(products);
        }

        [HttpGet("brands")]
        public async Task<ActionResult> GetProductsByBrand()
        {
            var products = await productService.GetAllBrandsAsync();
            return Ok(products);
        }
    }
}
