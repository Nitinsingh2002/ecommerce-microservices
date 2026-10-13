

using System.Collections.Generic;
using Authentication.Application.Abstractions.Authentication;
using Authentication.Domain.Identity;
using BuildingBlocks.SharedKernel.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Authentication.Infrastructure.Identity;

public class IdentityServices : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<IdentityServices> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public IdentityServices(
    UserManager<ApplicationUser> _userManager,
    ILogger<IdentityServices> _logger,
    SignInManager<ApplicationUser> _signInManager
    )
    {
        this._userManager = _userManager;
        this._logger = _logger;
        this._signInManager = _signInManager;
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

    public async Task<bool> AddToRoleAsync(Guid userId, string role, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if ((user == null))
        {
            return false;
        }

        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded;

    }


    public async Task<UserIdentityModel> CheckPasswordAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            _logger.LogWarning("Login failed: Account lookup failed");
            return null;
        }
        // for sign in use signIn manger to get lockout and other sign in policy check if we use userManager.CheckPasswordAsync it will not check for lockout and other sign in policy
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                _logger.LogWarning("Login failed for user :{email}, due to account lockout", email);
                return null;
            }
            else
            {
                _logger.LogWarning("Login failed for user :{email}, due to invalid credentails", email);
            }
            return null;
        }

        var response = new UserIdentityModel
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName
        };

        return response;
    }

    public async Task<UserIdentityModel> FindByEmailasync(string email, CancellationToken cancellationToken)
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
        if (user == null)
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