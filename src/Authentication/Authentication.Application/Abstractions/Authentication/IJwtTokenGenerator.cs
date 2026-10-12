using Authentication.Domain.Identity;
using BuildingBlocks.SharedKernel.Enums;
using System.Collections.Generic;
namespace Authentication.Application.Abstractions.Authentication;

public interface IJwtTokenGenrator
{
    public string GenrateToken(UserIdentityModel user, IReadOnlyCollection<UserRole> roles);
}

