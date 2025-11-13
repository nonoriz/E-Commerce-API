using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketService(IBasketRepository basketRepository,IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }
        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket)
        {
            var CustomerBasket=mapper.Map<BasketDTO,CustomerBasket>(basket);
            var CreatedOrUpdatedBasket= await basketRepository.CreateOrUpdateBasketAsync(CustomerBasket);
            return mapper.Map<CustomerBasket,BasketDTO>(CreatedOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string id)=>await basketRepository.DeleteBasketAsync(id);


        public async Task<BasketDTO> GetBasketAsync(string id)
        {
           var Basket=await basketRepository.GetBasketAsync(id);
            return  mapper.Map<CustomerBasket,BasketDTO>(Basket!);
        }
    }
}
