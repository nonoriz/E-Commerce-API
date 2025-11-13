using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.DataSeed
{
    public class DataInitialize : IDataInitializer
    {
        private readonly StoreDbContext dbContext;

        public DataInitialize(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {
            try
            {
              var HasProducts= await dbContext.Products.AnyAsync();
              var HasBrands= await dbContext.ProductBrands.AnyAsync();
                var HasTypes= await dbContext.ProductTypes.AnyAsync();
                if (HasBrands && HasProducts && HasTypes) return;
                if (!HasBrands)
                   await SeedDataFromJsonAsync<ProductBrand, int>("brands.json", dbContext.ProductBrands);
                if (!HasTypes)
                   await SeedDataFromJsonAsync<ProductType, int>("types.json", dbContext.ProductTypes);
              await  dbContext.SaveChangesAsync();

                if (!HasProducts)
                   await SeedDataFromJsonAsync<Product, int>("products.json", dbContext.Products);
               await dbContext.SaveChangesAsync();

               

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Data Seeeding Failed : {ex}");
            }
        }

        //D:\c# projects\E-CommerceSolution\E Commerce.Persistence\DataSeed\JSONFiles\brands.json

        private async Task SeedDataFromJsonAsync<T,TKey>(string fileName,DbSet<T> dbSet)where T : BaseEntity<TKey>
        {
            var filePath= @"..\E Commerce.Persistence\DataSeed\JSONFiles\"+fileName;
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File {fileName} is not Exist");

            try
            {
                using var DataStream = File.OpenRead(filePath);
                var data =await JsonSerializer.DeserializeAsync<List<T>>(DataStream, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                if(data is not null)
                {
                    dbSet.AddRange(data);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error while reading JSON File : {ex}");
                return;
            }
          
        }
    }
}
