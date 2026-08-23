

using Authentication.Domain.Identity;
using Authentication.Infrastructure.Persistence.seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastucture.Persistence.Seed;


public static class IdentitySeederExtensions{
    
    public static async Task SeedIdentityAsync( this IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        await IdentitySeeder.SeedAsync(roleManager);
    }
}