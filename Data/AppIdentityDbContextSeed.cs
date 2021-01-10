using AuthenticationClientService.API.Constants;
using AuthenticationClientService.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AuthenticationClientService.API.Data
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ILogger<AppIdentityDbContextSeed> logger)
        {
            try
            {
                await CreateRoles(roleManager);
                await CreateUsers(userManager);

                var adminUser = await userManager.FindByNameAsync("administradorusr");
                var operadorUser = await userManager.FindByNameAsync("operadorusr");
                var visualizadorUser = await userManager.FindByNameAsync("visualizadorusr");

                await userManager.AddToRoleAsync(adminUser, Roles.ADMINISTRADOR);
                await userManager.AddToRoleAsync(operadorUser, Roles.OPERADOR);
                await userManager.AddToRoleAsync(visualizadorUser, Roles.VISUALIZADOR);

                await userManager.GenerateEmailConfirmationTokenAsync(adminUser);
                await userManager.GenerateEmailConfirmationTokenAsync(operadorUser);
                await userManager.GenerateEmailConfirmationTokenAsync(visualizadorUser);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(AppIdentityDbContext));
            }
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            var adminRole = new IdentityRole(Roles.ADMINISTRADOR);
            var operadorRole = new IdentityRole(Roles.OPERADOR);
            var visualizadorRole = new IdentityRole(Roles.VISUALIZADOR);

            await roleManager.CreateAsync(adminRole);
            await roleManager.CreateAsync(operadorRole);
            await roleManager.CreateAsync(visualizadorRole);
        }

        private static async Task CreateUsers(UserManager<ApplicationUser> userManager)
        {
            var adminUser = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Administrador",
                LastName = "Master",
                Email = "adminmaster@email.com",
                PhoneNumber = "+55 (11)99999-9999",
                UserName = "administradorusr",
                NormalizedEmail = "ADMINMASTER@EMAILC.COM",
                EmailConfirmed = true,
                NormalizedUserName = "ADMINISTRADORUSR",
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };
            await userManager.CreateAsync(adminUser, "Admin@123");

            var operadorUser = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Operador",
                LastName = "User",
                Email = "operadordemo@email.com",
                PhoneNumber = "+55 (11)99999-9999",
                UserName = "operadorusr",
                NormalizedEmail = "OPERADORDEMO@email.com",
                EmailConfirmed = true,
                NormalizedUserName = "OPERADORUSR",
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };
            await userManager.CreateAsync(operadorUser, "Operador@123");

            var visualizadorUser = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Visualizador",
                LastName = "User",
                Email = "visualizadordemo@email.com",
                PhoneNumber = "+55 (11)99999-9999",
                UserName = "visualizadorusr",
                NormalizedEmail = "VISUALIZADORDEMO@email.com",
                EmailConfirmed = true,
                NormalizedUserName = "VISUALIZADORUSR",
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };
            await userManager.CreateAsync(visualizadorUser, "Visualizador@123");
        }
    }
}
