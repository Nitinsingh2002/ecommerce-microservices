using Authentication.Application.Abstractions.Authentication;
using MediatR;
namespace Authentication.Application.Features.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService _identityService)
    {
        this._identityService = _identityService;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand command, CancellationToken token)

    {
        var result = _identityService.CreateUserAsync(new CreateUserModel(command.FirstName,
        command.LastName, command.Email, command.Password), token);

        if (!result.Result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Result.Errors));
        }

        var user = await _identityService.FindByEmailasync(command.Email);

        if (user == null)
        {
            throw new InvalidOperationException("User was created but could not be retrieved.");
        }
        
             return new RegisterResponse(
            user.Id,
            user.Email);
    }
}