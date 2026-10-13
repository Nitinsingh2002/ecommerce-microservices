

namespace Authentication.Application.Abstractions.Authentication;

public interface IRefreshTokenGenrator
{
    public (string token, string hashedToken) GenerateRefreshToken();
}