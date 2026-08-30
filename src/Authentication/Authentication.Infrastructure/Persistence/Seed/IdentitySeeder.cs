

using Authentication.Domain.Identity;
using BuildingBlocks.SharedKernel.Enums;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Infrastructure.Persistence.seed;


public static class IdentitySeeder
{

    public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
    {
        // we are taking all the role from enum that we created in shaared kernel
        foreach ( var  roleNames in Enum.GetValues<UserRole>())
        {
            var roleName = roleNames.ToString();
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

