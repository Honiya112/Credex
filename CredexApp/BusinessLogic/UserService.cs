using CredexApp.Models;
using Microsoft.AspNetCore.Identity;

namespace CredexApp.BusinessLogic;

public class UserService
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<User>>();

        var admin = await userManager.FindByEmailAsync("secure@credexadmin.com");
        
        if (admin == null)
        {
            admin = new User {Email = "secure@credexadmin.com", UserName = "admin"};
            
            var result = await userManager.CreateAsync(admin, "adminPass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
        
    }
}