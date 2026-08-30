

using System.Collections.Generic;
using Authentication.Application.Abstractions.Authentication;
using Authentication.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Authentication.Infrastructure.Identity;

public class IdentityServices : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityServices(UserManager<ApplicationUser> _userManager)
    {
        this._userManager = _userManager;
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

    public async Task<IReadOnlyCollection<string>> GetRoleAsync(Guid UserId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(UserId.ToString());

        if (user == null)
        {
            return Array.Empty<string>();
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles == null || roles.Count == 0)
        {
            return Array.Empty<string>();
        }
        return roles.ToArray();
    }
}