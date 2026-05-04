using Ecommerce.API.Middlewares;
using Ecommerce.Core;
using Ecommerce.Core.Mappers;
using Ecommerce.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the application pipeline.

// Add infrastructure services to the dependency injection container.
builder.Services.AddInfrastructure();

// Add core services to the dependency injection container.
builder.Services.AddCore();

// Add controllers to the application pipeline.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Add support for serializing and deserializing enum values as strings in JSON.
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add auto mapper services to the dependency injection container, specifying the mapping profile assembly.
builder.Services.AddAutoMapper(cfg => { }, typeof(ApplicationUserMappingProfile).Assembly);

// Build the web application.
var app = builder.Build();

// Add custom exception handling middleware.
app.UseExceptionHandlingMiddleware();

// Add routing.
app.UseRouting();

// Add authentication and authorization middleware.
app.UseAuthentication();
app.UseAuthorization();

// Map controller endpoints.
app.MapControllers();

app.Run();
