using E_Commerce.Presentation.Attributes;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }
        [Authorize(Roles ="Admin")]
        [RedisCache]
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

            return HandleResult<ProductDTO>(product);
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
