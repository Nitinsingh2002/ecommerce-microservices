
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Authentication.Application.Features;

public sealed record LoginResponse
(
string AccessToke,
string RefreshToken,
DateTime AccessTokenExpiration,
DateTime RefreshTokenExpiration,
Guid UserId
);
