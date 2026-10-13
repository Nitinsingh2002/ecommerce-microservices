

using Authentication.Domain.Identity;
namespace Authentication.Application.Abstractions.Authentication;

public interface IRefreshTokenRepository
{
    public Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);

    public Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
}