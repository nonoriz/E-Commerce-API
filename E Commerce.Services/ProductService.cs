using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Specifications;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared;
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

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Repo = unitOfWork.GenericRepository<Product, int>();
            var spec =new ProductWithTypeAndBrandSpecification(queryParams);
            var products= await Repo.GetAllAsync(spec);
            var DataToReturn= mapper.Map<IEnumerable<ProductDTO>>(products);
            var CountOfReturnedData = DataToReturn.Count();
            var CountSpec = new ProductCountSpecification(queryParams);
            var CountOfAllProducts=await Repo.CountAsync(CountSpec);
            return new PaginatedResult<ProductDTO>(queryParams.PageIndex, CountOfReturnedData, CountOfAllProducts, DataToReturn);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {

            var types=await unitOfWork.GenericRepository<ProductType, int>().GetAllAsync();
            return mapper.Map<IEnumerable<TypeDTO>>(types);
        }


        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithTypeAndBrandSpecification(id);
            var product=await unitOfWork.GenericRepository<Product, int>().GetByIdAsync(spec);
            return mapper.Map<ProductDTO>(product);
        }
    }
}
