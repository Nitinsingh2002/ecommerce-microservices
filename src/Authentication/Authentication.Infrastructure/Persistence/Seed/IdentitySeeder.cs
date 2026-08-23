

using Authentication.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Infrastructure.Persistence.seed;


public static class IdentitySeeder
{
    private static readonly string[] Roles = { "Admin", "Vendor", "Customer"};

    public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
    {
        foreach ( var  roleName in Roles)
        {
            if( await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            var role = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = roleName,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var result = await roleManager.CreateAsync(role);

            if(!result.Succeeded)
            {
                var errors = string.Join(", ",result.Errors.Select(x => x.Description));

                throw new InvalidOperationException($"Failed to create role :{roleName} : {errors}");
            }
        }
    }
}

