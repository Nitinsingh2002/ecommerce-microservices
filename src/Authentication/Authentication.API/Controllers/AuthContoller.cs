using Authentication.Api.Contracts;
using Authentication.Api.Contracts.register;
using Authentication.Api.Contracts.register;
using Authentication.Application.Features.Login;
using Authentication.Application.Features.Register;
using Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
namespace Authentication.Api.Contollers;


[ApiController]
[Route("/api/v1/[controller]")]
public class AuthContoller : ControllerBase
{

    private readonly ISender _sender;
    private readonly ILogger<AuthContoller> logger;

    public AuthContoller(ISender _sender, ILogger<AuthContoller>logger)
    {
        this._sender = _sender;
        this.logger = logger;
    }


    [HttpPost("/register")]
    public async Task<ActionResult<RegisterResponseDTO>> Register([FromBody] RegisterRequest request, CancellationToken token
        )
    {
        logger.LogInformation("Registration HTTP Request Recieved, Email: {email}", request.Email.ToString());
        var command = new RegisterCommand
        (
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password
        );

        var result = await _sender.Send(command, token);

        // using centralized error response class to return error response in a structured format.
        if (result.IsFailure)
        {
            logger.LogError("Registaration HTTP Request Faiiled. Email: {email}, Error code: {errorcode} ",
               request.Email, result.Error.code);

            var ErrorResponse = new ApiErrorResponse(
                new ApiError(result.Error.code, result.Error.message)
            );

            return BadRequest(ErrorResponse);
        }

        logger.LogInformation("Registration HTTP request completed successfully. user Id: {userid}", result.Value.UserId);
        var response = new RegisterResponseDTO(request.Email, result.Value.UserId);

        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPost("/login")]
    [EnableRateLimiting("LoginPolicy")]
    public async Task<ActionResult> Login([FromBody] LoginRequestDTO request, CancellationToken token)
    {
        logger.LogInformation("Login HTTP Request Recieved, Email: {email}", request.Email);

        //converting the request to a command

        var command = new LoginCommand
        (
            request.Email,
            request.Password
        );

        var result = await _sender.Send(command, token);

        if (result.IsFailure)
        {
            var errrorResponse = new ApiErrorResponse(
                new ApiError(
                result.Error.code, result.Error.message
                ));

            return Unauthorized(errrorResponse);
        }
        var response = new LoginResponseDTO(
            AccessToken: result.Value.AccessToke,
            RefreshToke: result.Value.RefreshToken,
            AccessTokenExpiration: result.Value.AccessTokenExpiration,
            RefreshTokenExpiration: result.Value.RefreshTokenExpiration
            );
        logger.LogInformation("Login HTTP Requrst completed successfully");
        return Ok(response);
    }

}

