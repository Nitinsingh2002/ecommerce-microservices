

namespace Authentication.Api.Contracts.register;
public sealed record RegisterRequest(string FirstName, string LastName, string Email, string Password);