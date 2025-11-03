using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var Brands=await unitOfWork.GenericRepository<ProductBrand, int>().GetAllAsync();
            return mapper.Map<IEnumerable<BrandDTO>>(Brands);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products= await unitOfWork.GenericRepository<Product, int>().GetAllAsync();
            return mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var types=await unitOfWork.GenericRepository<ProductType, int>().GetAllAsync();
            return mapper.Map<IEnumerable<TypeDTO>>(types);
        }


        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product=await unitOfWork.GenericRepository<Product, int>().GetByIdAsync(id);
            return mapper.Map<ProductDTO>(product);
        }
    }
}
