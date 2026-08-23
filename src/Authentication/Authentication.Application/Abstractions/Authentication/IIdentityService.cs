using Authentication.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Application.Abstractions.Authentication;
public interface IIdentityService
{
    public Task<IdentityResultModel> CreateUserAsync(CreateUserModel user, CancellationToken cancellationToken);
    public Task<bool> CheckPasswordAsync(string email, string password, string cancellationToken);
    public Task<UserIdentityModel> FindByEmailasync(string email);
    public Task<IReadOnlyCollection<string>> GetRoleAsync(Guid UserId, string cancellationToken);
    public Task<bool> CheckEmailExist(string email);

}