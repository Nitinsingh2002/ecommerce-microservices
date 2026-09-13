using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Authentication.Application.Behaviour;
namespace Authentication.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

            //Registering the pipeline behavior for validation
            configuration.AddOpenBehavior(typeof(ValidatorBehaviour<,>));
        });
        return services;
    }
}