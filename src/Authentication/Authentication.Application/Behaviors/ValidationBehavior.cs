

using System.Net.Http.Headers;
using FluentValidation;
using MediatR;
namespace Authentication.Application.Behaviour;


public class ValidatorBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest,TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _Validators;

    public ValidatorBehaviour(IEnumerable<IValidator<TRequest>> _Validators)
    {
        this._Validators = _Validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse>next, CancellationToken cancellationToken)
    {
        if (!_Validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(_Validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var ValidationFailures = validationResults.SelectMany(result => result.Errors)
        .Where(error => error is not null).ToList();

        if (ValidationFailures.Count != 0)
        {
            throw new ValidationException(ValidationFailures);
        }
        return await next();
    }
    
}