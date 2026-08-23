namespace Authentication.Application.Abstractions.Authentication;

public sealed class  UserIdentityModel
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;

}