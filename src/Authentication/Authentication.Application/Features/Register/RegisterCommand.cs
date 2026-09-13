
using BuildingBlock.SharedKernel.Results;
using BuildingBlocks.SharedKernel.Results;
using MediatR;
namespace Authentication.Application.Features.Register;

public sealed record RegisterCommand(string FirstName, string LastName, string Email, string Password)
: IRequest<Result<RegisterResult>>
{

};