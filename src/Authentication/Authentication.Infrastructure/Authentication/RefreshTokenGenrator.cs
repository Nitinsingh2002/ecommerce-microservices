

using Authentication.Application.Abstractions.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace Authentication.Infrastructure.Authentication;

public class RefreshTokenGenrator : IRefreshTokenGenrator
{
    public (string token, string hashedToken) GenerateRefreshToken()
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var hashedToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
        return (token, hashedToken);

        //plain token is returned to user and hashed token is stored in database for security purpose
    }
}