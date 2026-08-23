namespace Authentication.Application.Abstractions.Authentication;

public sealed record CreateUserModel
(
    string FirstName,
    string LastName,
    string Email,
    string Password
);

