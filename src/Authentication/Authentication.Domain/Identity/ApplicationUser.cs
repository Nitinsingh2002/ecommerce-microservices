
using Microsoft.AspNetCore.Identity;

namespace Authentication.Domain.Identity;
public sealed class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get;set;} = string.Empty;
    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
    public DateTime? UpdatedAt {get;set;} = DateTime.UtcNow;
    public bool IsDeleted {get;set;} = false;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

}