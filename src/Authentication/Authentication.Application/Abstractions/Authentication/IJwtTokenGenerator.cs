using Authentication.Domain.Identity;
namespace Authentication.Application.Abstractions.Authentication;

public interface IJwtTokenGenrator
{
    public string GenrateToken(ApplicationUser user);
}

