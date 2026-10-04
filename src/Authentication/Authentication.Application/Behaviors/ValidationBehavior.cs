

using System.Net.Http.Headers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
namespace Authentication.Application.Behaviour;


public class ValidatorBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest,TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _Validators;
    private readonly ILogger<ValidatorBehaviour<TRequest, TResponse>> _logger;


    public ValidatorBehaviour(IEnumerable<IValidator<TRequest>> _Validators, ILogger<ValidatorBehaviour<TRequest,TResponse>>_logger)
    {
        this._Validators = _Validators;
        this._logger = _logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse>next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        if (!_Validators.Any())
        {
            _logger.LogDebug("No Validators registered for the request {request}", requestName);
            return await next();
        }

        _logger.LogDebug("Validation started for request: {request}", requestName);

        //fluent validation context that contain the request that being validated.
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(_Validators.Select(v => v.ValidateAsync( context, cancellationToken)));

        var ValidationFailures = validationResults.SelectMany(result => result.Errors)
        .Where(error => error is not null).ToList();

        if (ValidationFailures.Count != 0)
        {
            _logger.LogWarning("Validationn failed for the request: {requestName} with count: {count}", requestName, ValidationFailures.Count
                ());

            foreach (var failure in ValidationFailures)
            {
                _logger.LogWarning("Validation failure. Request: {RequestName}, Property: {PropertyName}," +
                    " ErrorCode: {ErrorCode}", requestName, failure.PropertyName, failure.ErrorCode);
            }

            throw new ValidationException(ValidationFailures);
        }

        _logger.LogDebug("Validation completed sucessfully for the request: {request}", requestName);
        return await next();
    }
    
}