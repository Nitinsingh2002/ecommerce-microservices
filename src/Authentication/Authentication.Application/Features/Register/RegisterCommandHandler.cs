using Authentication.Application.Abstractions.Authentication;
using MediatR;
namespace Authentication.Application.Features.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public string defaultRole = "Customer";
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService _identityService)
    {
        this._identityService = _identityService;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand command, CancellationToken token)

    {
        //checking user is already avialable with email or not
        var existingUser = await _identityService.FindByEmailasync(command.Email, token);

        if (existingUser != null)
        {
            throw new InvalidOperationException("User already availble with email");
        }

        //creating user 
        var result = await _identityService.CreateUserAsync(new CreateUserModel(command.FirstName,
            command.LastName, command.Email, command.Password), token);

        if (!result.Succeeded || result.UserId is null)
        {
            throw new InvalidOperationException(
                string.Join(", ", result.Errors));
        }

        //assigning default role to user
        var rolesAssigned = await _identityService.AddToRoleAsync(result.UserId.Value,defaultRole, token);

        if (!rolesAssigned)
        {
            throw new InvalidOperationException("User is created but role is not assigned");
        }

        return new RegisterResponse(result.UserId.Value, command.Email);
    }
}