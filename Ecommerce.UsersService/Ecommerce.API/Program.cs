using Ecommerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the application pipeline.

// Add infrastructure services to the dependency injection container.
builder.Services.AddInfrastructure();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
