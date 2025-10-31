using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Persistence.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Data.DbContexts
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           
        }

        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<ProductBrand> ProductBrands { get; set; } = default!;
        public DbSet<ProductType> ProductTypes { get; set; } = default!;
    }
}
