using Authentication.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Application.Abstractions.Authentication;
public interface IIdentityService
{
    public Task<IdentityResultModel> CreateUserAsync(CreateUserModel user, CancellationToken cancellationToken);
    public Task<bool> CheckPasswordAsync(string email, string password, CancellationToken cancellationToken);
    public Task<UserIdentityModel> FindByEmailasync(string email, CancellationToken cancellationToken);
    public Task<IReadOnlyCollection<string>> GetRoleAsync(Guid UserId, CancellationToken cancellationToken);
    public Task<bool> CheckEmailExist(string email);


    public Task<bool> AddToRoleAsync(Guid userId, string role, CancellationToken cancellationToken);

}