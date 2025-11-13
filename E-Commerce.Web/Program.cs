
using E_Commerce.Domain.Contracts;
using E_Commerce.Persistence.Data.DbContexts;
using E_Commerce.Persistence.DataSeed;
using E_Commerce.Persistence.Repositories;
using E_Commerce.Services;
using E_Commerce.Services.MappingProfiles;
using E_Commerce.Services_Abstraction;
using E_Commerce.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            builder.Services.AddScoped<IDataInitializer, DataInitialize>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(ServiceAssemplyReference).Assembly);
            //builder.Services.AddTransient<ProductPictureUrlResolver>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddSingleton(sp =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();

            #endregion

            var app = builder.Build();
            #region Data Seeding
            await app.MigerateDatabsaeAsync();
            await app.SeedDatabaseAsync();
            
           
            #endregion


            #region Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            await app.RunAsync();
        }
    }
}
