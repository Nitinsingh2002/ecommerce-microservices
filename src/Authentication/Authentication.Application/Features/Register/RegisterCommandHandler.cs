using Authentication.Application.Abstractions.Authentication;
using Authentication.Application.Features.Register;
using BuildingBlock.SharedKernel.Results;
using BuildingBlocks.SharedKernel.Results;
using  BuildingBlocks.SharedKernel.Results;
using MediatR;
namespace Authentication.Application.Features.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResult>>
{
    public string defaultRole = "Customer";
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService _identityService)
    {
        this._identityService = _identityService;
    }

    public async Task<Result<RegisterResult>> Handle(RegisterCommand command, CancellationToken token)

    {
        //checking user is already avialable with email or not
        var existingUser = await _identityService.FindByEmailasync(command.Email, token);

        // if (existingUser != null)
        // {
        //     throw new InvalidOperationException("User already availble with email");
        // }

        // Instead of throwing exception, we are using result pattern in result class we have defined Failure menthod which 
        // take Error object and return result with failure status and error message.

        if (existingUser != null)
        {
            return Result<RegisterResult>.Failure(new Error("Authentication.UserAlreadyExists", "A user with this email already exists."));
        }

        //creating user 
        var result = await _identityService.CreateUserAsync(new CreateUserModel(command.FirstName,
            command.LastName, command.Email, command.Password), token);

        if (!result.Succeeded || result.UserId is null)
        {
            return Result<RegisterResult>.Failure(new Error("Authentication.UserCreationFailed",
                    "The user could not be created."));
        }

        //assigning default role to user
        var rolesAssigned = await _identityService.AddToRoleAsync(result.UserId.Value, defaultRole, token);

        if (!rolesAssigned)
        {
            return Result<RegisterResult>.Failure(new Error("Authentication.RoleAssignmentFailed",
                     "The user could not be assigned to the default role."));
        }

        // return Result<RegisterResult>.Success(new RegisterResult(result.UserId.Value, command.Email));

            var Regresult = new RegisterResult(
            result.UserId.Value,
            command.Email);

        return Result<RegisterResult>.Success(Regresult);
    }
}