using Authentication.Api.Contracts.register;
using Authentication.Api.Contracts.register;
using Authentication.Application.Features.Register;
using BuildingBlocks.SharedKernel.Results;
using Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
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


    [HttpPost("/register")]
    public async Task<ActionResult<RegisterResponseDTO>> Register([FromBody] RegisterRequest request, CancellationToken token
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

        // if (result.IsFailure)
        // {
        //     return BadRequest(new
        //     {
        //         error = new
        //         {
        //             code = result.Error.code,
        //             message = result.Error.message
        //         }
        //     });
        // }


        // using centralized error response class to return error response in a structured format.
        if (result.IsFailure)
        {
            var ErrorResponse = new ApiErrorResponse(
                new ApiError(result.Error.code, result.Error.message)
            );

            return BadRequest(ErrorResponse);
        }

        var response = new RegisterResponseDTO(request.Email, result.Value.UserId);

        return StatusCode(StatusCodes.Status201Created, response);
    }

}

