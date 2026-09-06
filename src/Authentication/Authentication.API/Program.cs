using Authentication.Application.DependencyInjection;
using Authentication.Infrastructure.DependencyInjection;
using Common.Extensions;
using Infrastucture.Persistence.Seed;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// both are same here we consuming all the DI in commented there is extension menthod approch
builder.Services.AddApplication();
builder.Services.addInfrastructure(builder.Configuration);

//ApplicationServiceRegistration.AddApplication(builder.Services);
//InfrastructureServiceRegistration.addInfrastructure(builder.Services, builder.Configuration);

var app = builder.Build();

app.UseGlobalExceptionalHandler();
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();