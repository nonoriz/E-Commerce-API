using E_Commerce.Domain.Contracts;
using E_Commerce.Services_Abstraction;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            this.cacheRepository = cacheRepository;
        }
        public async Task<string?> GetAsync(string CacheKey)
        {
           return await cacheRepository.GetAsync(CacheKey);
        }

        public async Task SetAsync(string CacheKey, string CacheValue, TimeSpan TimeToLive)
        {
            var value=JsonSerializer.Serialize(CacheValue,new JsonSerializerOptions()
            {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase
            });
             await cacheRepository.SetAsync(CacheKey, value, TimeToLive);

        }


      
    }
}
