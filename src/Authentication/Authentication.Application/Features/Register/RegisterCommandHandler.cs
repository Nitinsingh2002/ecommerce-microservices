using Authentication.Application.Abstractions.Authentication;
using Authentication.Application.Features.Register;
using BuildingBlock.SharedKernel.Results;
using BuildingBlocks.SharedKernel.Results;
using  BuildingBlocks.SharedKernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;
namespace Authentication.Application.Features.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResult>>
{
    public string defaultRole = "Customer";
    private readonly IIdentityService _identityService;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(IIdentityService _identityService,ILogger<RegisterCommandHandler>_logger)
    {
        this._identityService = _identityService;
        this._logger = _logger;
    }

    public async Task<Result<RegisterResult>> Handle(RegisterCommand command, CancellationToken token)

    {
        _logger.LogInformation("User Registration started, Email: {email}", command.Email);

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
            _logger.LogWarning("Registration request rejected because email already exist, Email: {email}", command.Email);
            return Result<RegisterResult>.Failure(new Error("Authentication.UserAlreadyExists", "A user with this email already exists."));
        }

        //creating user 
        var result = await _identityService.CreateUserAsync(new CreateUserModel(command.FirstName,
            command.LastName, command.Email, command.Password), token);

        if (!result.Succeeded || result.UserId is null)
        {
            _logger.LogError("User creation failed, Email: {email}, Error code: {error code}",command.Email,result.Errors);

            return Result<RegisterResult>.Failure(new Error("Authentication.UserCreationFailed",
                    "The user could not be created."));
        }

        _logger.LogInformation("User created successfully, Email: {email}", command.Email);
        _logger.LogInformation("Role assigning to user");
        //assigning default role to user
        var rolesAssigned = await _identityService.AddToRoleAsync(result.UserId.Value, defaultRole, token);

        if (!rolesAssigned)
        {
            _logger.LogError("Role assignment failed, userID: {user} Role: {role}", result.UserId.Value, defaultRole);
            return Result<RegisterResult>.Failure(new Error("Authentication.RoleAssignmentFailed",
                     "The user could not be assigned to the default role."));
        }

        // return Result<RegisterResult>.Success(new RegisterResult(result.UserId.Value, command.Email));

            var Regresult = new RegisterResult(
            result.UserId.Value,
            command.Email);
        _logger.LogInformation("User registerd completed successfuly Email: {email}", command.Email);
        return Result<RegisterResult>.Success(Regresult);
    }
}