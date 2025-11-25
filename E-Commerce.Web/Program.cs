
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Persistence.Data.DbContexts;
using E_Commerce.Persistence.DataSeed;
using E_Commerce.Persistence.IdentityData.DataSeed;
using E_Commerce.Persistence.IdentityData.DbContexts;
using E_Commerce.Persistence.Repositories;
using E_Commerce.Services;
using E_Commerce.Services.MappingProfiles;
using E_Commerce.Services_Abstraction;
using E_Commerce.Web.CustomMiddleWares;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
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
            builder.Services.AddKeyedScoped<IDataInitializer, DataInitialize>("Default");
            builder.Services.AddKeyedScoped<IDataInitializer, IdentityDataInitializer>("Identity");
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
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
            });
            builder.Services.AddDbContext<StoreIdentityDbContext>(options=>
            options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"))
            );
            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:Audiene"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecurtyKey"]))
                };
            });
            #endregion

            var app = builder.Build();
            #region Data Seeding
            await app.MigerateDatabaseAsync();
            await app.MigerateIdentityDatabaseAsync();
            await app.SeedDatabaseAsync();
            await app.SeedIdentityDatabaseAsync();


            #endregion


            #region Configure the HTTP request pipeline.

            //app.Use(async (Context, next) =>
            //{
            //    try
            //    {
            //        await next();

            //    }catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        Context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            //        await Context.Response.WriteAsJsonAsync(new
            //        {
            //            StatusCodes=StatusCodes.Status500InternalServerError,
            //            Error=$"An unexpected error occured. {ex.Message}"

            //        });
            //    }


            //});
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            await app.RunAsync();
        }
    }
}
