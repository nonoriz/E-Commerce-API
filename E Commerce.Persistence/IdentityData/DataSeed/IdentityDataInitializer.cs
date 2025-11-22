using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.IdentityData.DataSeed
{
    public class IdentityDataInitializer : IDataInitializer
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<IdentityDataInitializer> logger;

        public IdentityDataInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,ILogger<IdentityDataInitializer> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }
        public async Task InitializeAsync()
        {
            try
            {
                if (!roleManager.Roles.Any())
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

                }
                if (!userManager.Users.Any())
                {
                    var User01 = new ApplicationUser
                    {
                        DisplayName = "Mohamed Tarek",
                        UserName = "MohamedTarek",
                        Email = "MohamedTarek@gmail.com",
                        PhoneNumber = "01101943883"
                    };
                    var User02 = new ApplicationUser
                    {
                        DisplayName = "Salma Tarek",
                        UserName = "SalmaTarek",
                        Email = "SalmaTarek@gmail.com",
                        PhoneNumber = "01101943884"

                    };
                    
                    await userManager.CreateAsync(User01, "Pa$$w0rd");
                    await userManager.CreateAsync(User02, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(User01, "Admin");
                    await userManager.AddToRoleAsync(User02, "SuperAdmin");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error While seeding identity database :Message ={ex.Message}");
            }
        }
    }
}
