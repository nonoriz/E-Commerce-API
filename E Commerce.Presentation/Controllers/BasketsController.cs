using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController:ControllerBase
    {
        private readonly IBasketService basketService;

        public BasketsController(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketAsync(string id)
        {
            var Basket = await basketService.GetBasketAsync(id);
            return Ok(Basket);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBasketAsync(BasketDTO basket)
        {
            var Basket = await basketService.CreateOrUpdateBasketAsync(basket);
            return Ok(Basket);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketAsync(string id)
        {
            var Result = await basketService.DeleteBasketAsync(id);
            return Ok(Result);
        }

    }
}
