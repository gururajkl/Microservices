using Ecommerce.Core;
using Ecommerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the application pipeline.

// Add infrastructure services to the dependency injection container.
builder.Services.AddInfrastructure();

// Add core services to the dependency injection container.
builder.Services.AddCore();

// Add controllers to the application pipeline.
builder.Services.AddControllers();

// Build the web application.
var app = builder.Build();

// Add routing.
app.UseRouting();

// Add authentication and authorization middleware.
app.UseAuthentication();
app.UseAuthorization();

// Map controller endpoints.
app.MapControllers();

app.Run();
