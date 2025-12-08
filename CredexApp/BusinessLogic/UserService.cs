using CredexApp.Models;
using Microsoft.AspNetCore.Identity;

namespace CredexApp.BusinessLogic;

public class UserService
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

         string[] roles = {"Admin"};

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        
        CreateAdmin(userManager);
    }

    public static async void CreateAdmin(UserManager<User> userManager)
    {
        var adminRoleName = "Admin";
        var adminEmail = "admin@credex.com";
        var adminPassword = "admin";
        
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        
        if (adminUser == null)
        {
            adminUser = new User
            {
                Email = adminEmail,
                UserName = adminEmail,
                EmailConfirmed = true,
            };
            
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRoleName);
            }
        }
    }
}