using Authentication.Api.Contracts.register;
using Authentication.Application.Features.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Authentication.Api.Contollers;

[ApiController]
[Route("/api/v1/[controller]")]
public class AuthContoller : ControllerBase
{

    private readonly ISender _sender;

    public AuthContoller(ISender _sender)
    {
        this._sender = _sender;
    }


    [HttpPost]
    [ProducesResponseType(
        typeof(RegisterResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task <ActionResult<RegisterResponseDTO>> Register([FromBody] RegisterRequest request, CancellationToken token
        )
    {
        var command = new RegisterCommand
        (
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password
        );

        var result = await _sender.Send(command, token);

        var response = new RegisterResponseDTO(result.Email,result.UserId);
        return StatusCode(StatusCodes.Status201Created, response);
    }


}

