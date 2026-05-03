using Ecommerce.Core;
using Ecommerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the application pipeline.

// Add infrastructure services to the dependency injection container.
builder.Services.AddInfrastructure();

// Add core services to the dependency injection container.
builder.Services.AddCore();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
