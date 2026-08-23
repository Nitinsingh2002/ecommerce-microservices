
using MediatR;
namespace Authentication.Application.Features.Register;

public sealed record RegisterCommand(string FirstName, string LastName, string Email, string Password) : IRequest<RegisterResponse>
{

};