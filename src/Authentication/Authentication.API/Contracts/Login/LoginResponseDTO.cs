

namespace Authentication.Api.Contracts;

public sealed record LoginResponseDTO(
string AccessToken,
string RefreshToke,
DateTime AccessTokenExpiration,
DateTime RefreshTokenExpiration
);
