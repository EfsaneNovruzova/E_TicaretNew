namespace E_TicaretNew.Application;


using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using E_TicaretNew.Domain.Entities;  // User classının namespace-ni öz layihənə uyğun düzəlt
using static E_TicaretNew.Application.Shared.Permissions;
using E_TicaretNew.Application.Shared;
using E_TicaretNew.Application.Shared.Helpers;
using System.Security.Claims;   // Claim sinfi üçün
using Microsoft.AspNetCore.Identity;  // RoleManager və AddClaimAsync üçün


public static class SeedData
{


    public static async Task CreateAdminUser(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string adminEmail = "novruzovafsan88@gmail.com";
        string adminPassword = "12345@eN";

        // 1. Admin rolunu yoxla, yoxdursa yarat
        var adminRole = await roleManager.FindByNameAsync("Admin");
        if (adminRole == null)
        {
            adminRole = new IdentityRole("Admin");
            await roleManager.CreateAsync(adminRole);
        }

        // 2. Admin istifadəçini yoxla, yoxdursa yarat
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User
            {
                Email = adminEmail,
                UserName = adminEmail,
                FulName = "Administrator"
            };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Admin user creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        // 3. Admin istifadəçiyə Admin rolunu əlavə et
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        // 4. Admin roluna lazımi icazələri əlavə et (claims)
        var claims = await roleManager.GetClaimsAsync(adminRole);

        var permissions = PermissionHelper.GetAllPermissionList();  // bütün icazələri al

        foreach (var permission in permissions)
        {
            if (!claims.Any(c => c.Type == "Permission" && c.Value == permission))
            {
                await roleManager.AddClaimAsync(adminRole, new Claim("Permission", permission));
            }
        }
    }

}

