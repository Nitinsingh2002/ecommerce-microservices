using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Domain.Identity;
using Authentication.Infrastructure.Configurations;
using Authentication.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Authentication.Application.Abstractions.Authentication;
using Authentication.Infrastructure.Authentication;
using Authentication.Infrastructure.Identity;

namespace Authentication.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection addInfrastructure ( this IServiceCollection services, IConfiguration configurations)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                
                options.UseSqlServer(configurations.GetConnectionString("DefautConnection"));
            });

            services.Configure<JwtSettings>(configurations.GetSection("Jwt"));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.User.RequireUniqueEmail = true;
            })
              .AddEntityFrameworkStores<ApplicationDbContext>()

              .AddDefaultTokenProviders();

            services.AddScoped<IIdentityService, IdentityServices>();
            services.AddScoped<IJwtTokenGenrator,JwtTokenGenerator>();

            return services;

        }
    }
}
