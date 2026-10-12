
using MediatR;
using BuildingBlock.SharedKernel.Results;
using  Authentication.Application.Features;
namespace Authentication.Application.Features.Login;


public sealed record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;