
using System.Security.AccessControl;
using System.Security.Cryptography;
using Authentication.Application.Abstractions.Authentication;
using Authentication.Application.Features;
using Authentication.Domain.Identity;
using BuildingBlock.SharedKernel.Results;
using BuildingBlocks.SharedKernel.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Authentication.Application.Features.Login;


public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenGenrator _jwtTokenGenrator;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<LoginHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration configuration;
    private readonly IRefreshTokenGenrator refreshTokenGenrator;

    public LoginHandler(IIdentityService _identityService,
        IJwtTokenGenrator _jwtTokenGenrator,
        IRefreshTokenRepository _refreshTokenRepository,
        ILogger<LoginHandler> _logger,
        IUnitOfWork _unitOfWork,
        IConfiguration configuration,
        IRefreshTokenGenrator refreshTokenGenrator
        )
    {
        this._logger = _logger;
        this._jwtTokenGenrator = _jwtTokenGenrator;
        this._refreshTokenRepository = _refreshTokenRepository;
        this._unitOfWork = _unitOfWork;
        this.configuration = configuration;
        this._identityService = _identityService;
        this.refreshTokenGenrator = refreshTokenGenrator;
    }


    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("login attempt started email: {email}", command.Email);

        var user = await _identityService.CheckPasswordAsync(command.Email, command.Password, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("Login failed for user :{email}, due to invalid credentails", command.Email);
            return Result<LoginResponse>.Failure(new Error("AUTH.INVALID_CREDENTIALS", "Invalid login or password"));
        }

        var roles = await _identityService.GetRoleAsync(user.Id, cancellationToken);

        var accessToken = _jwtTokenGenrator.GenrateToken(user, roles);

        //genrating the refresh token we will send plain token to user and hashed token will be stored in DB for security purpose
        var (token, hashedToken) = refreshTokenGenrator.GenerateRefreshToken();

        //adding refresh token expiry time (Needs to configured in appsetting.json currently hardcoded value)
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(int.Parse(configuration["Jwt:RefreshTokenExpiryDays"]));

        //creating refresh token (entity)
        RefreshToken rtoken = new RefreshToken
        {
            Token = hashedToken,
            ExpiredAt = refreshTokenExpiry,
            IsRevoked = false,
            UserId = user.Id,
        };

        var accessTokenExpiryTime = DateTime.UtcNow.AddMinutes(int.Parse(configuration["Jwt:AccessTokenExpiryMinutes"]));

        //saving refresh token in  DB 
        await _refreshTokenRepository.AddAsync(rtoken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);


        var response = new LoginResponse(AccessToke: accessToken,
            RefreshToken: token,
            //chnage the hardcoded value 
            AccessTokenExpiration: accessTokenExpiryTime,
            RefreshTokenExpiration: refreshTokenExpiry,
            UserId: user.Id);

        _logger.LogInformation("Login completed successfully");

        return Result<LoginResponse>.Success(response);
    }
}