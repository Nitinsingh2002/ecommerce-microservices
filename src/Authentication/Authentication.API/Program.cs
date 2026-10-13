using Authentication.Application.DependencyInjection;
using Authentication.Infrastructure.DependencyInjection;
using Common.Extensions;
using Infrastucture.Persistence.Seed;
using Serilog;
using Authentication.Api.DependencyInjection;

//  serilog configuration
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Satrting Authentication API");

    // Add services to the container.
    builder.Services.AddSwaggerGen();

    // Configure Serilog from appsettings.json
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();
    });

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    // builder.Services.AddOpenApi();

    // both are same here we consuming all the DI in commented there is extension menthod approch
    builder.Services.AddApplication();
    builder.Services.addInfrastructure(builder.Configuration);

    //To call the Ratelimmiting confiuration
    builder.Services.AddApiServices();

    //ApplicationServiceRegistration.AddApplication(builder.Services);
    //InfrastructureServiceRegistration.addInfrastructure(builder.Services, builder.Configuration);

    var app = builder.Build();

    //calling out custom midlewares.
    app.UseGlobalExceptionalHandler();
    app.UseCorrelationId();
    app.UseRequestResponseLogging();


    // calling Role seeder
    await app.Services.SeedIdentityAsync();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        // app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRateLimiter();
    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Authentication API Started Suceessfully");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Authentication API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

