

using BuildingBlocks.SharedKernel.Enums;
using System.Collections.Generic;
using Authentication.Application.Abstractions.Authentication;
using Authentication.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Authentication.Infrastructure.Identity;

public class IdentityServices : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<IdentityServices> _logger;

    public IdentityServices(UserManager<ApplicationUser> _userManager,ILogger<IdentityServices> _logger)
    {
        this._userManager = _userManager;
        this._logger = _logger;
    }
    public async Task<IdentityResultModel> CreateUserAsync(CreateUserModel userModel, CancellationToken token)
    {
        var user = new ApplicationUser
        {
            FirstName = userModel.FirstName,
            LastName = userModel.LastName,
            Email = userModel.Email,
            UserName = userModel.Email,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, userModel.Password);

        return new IdentityResultModel
        {
            Succeeded = result.Succeeded,



            UserId = user.Id,
            Errors = result.Errors.Select(err => err.Description).ToArray()
        };

    }

    public async Task <bool> AddToRoleAsync (Guid userId, string role, CancellationToken cancellationToken){
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if ((user== null))
        {
            return false;
        }

        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded;

    }


    public async Task<bool> CheckPasswordAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return false;
        }

        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<UserIdentityModel> FindByEmailasync(string email,CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return null;
        }

        return new UserIdentityModel
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty
        };
    }

    public async Task<bool> CheckEmailExist(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if(user == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public async Task<IReadOnlyCollection<UserRole>> GetRoleAsync(Guid UserId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(UserId.ToString());

        if (user == null)
        {
            return Array.Empty<UserRole>();
        }

        //getRoleAsync will return the roles in string format so we need to convert it to UserRole enum
        IList<string>? roles = await _userManager.GetRolesAsync(user);

        if (roles == null || roles.Count == 0)
        {
            return Array.Empty<UserRole>();
        }

        //converting the string roles to userRole enum using Enum.Parse method and returning it as an array
        var userRoles = roles.Select(role => Enum.Parse<UserRole>(role));
        return userRoles.ToArray();
    }
}