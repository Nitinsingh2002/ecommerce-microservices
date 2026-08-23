using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Authentication.Application.Abstractions.Authentication;
using Authentication.Domain.Identity;
using Authentication.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Authentication.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenrator
{
    private readonly JwtSettings _jwtsettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtsetting)
    {
        _jwtsettings = jwtsetting.Value;
    }
    public string GenrateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
          new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
          new (JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
          new (ClaimTypes.Name, user.Id.ToString()),
          new(ClaimTypes.Name, user.UserName ?? string.Empty),
          new (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtsettings.SecretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
        issuer: _jwtsettings.Issuer,
        audience: _jwtsettings.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(_jwtsettings.AccessTokenExpiryMinutes),
        signingCredentials: credentials
        );

        string tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenstring;

    }
}

