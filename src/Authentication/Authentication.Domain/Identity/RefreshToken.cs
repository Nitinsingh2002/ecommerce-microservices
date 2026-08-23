namespace Authentication.Domain.Identity;

using SharedKernel.Primitives;

public class RefreshToken : AuditableEntity
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiredAt { get; set; }
    public bool IsRevoked { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
} 