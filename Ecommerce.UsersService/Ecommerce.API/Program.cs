using Ecommerce.API.Middlewares;
using Ecommerce.Core;
using Ecommerce.Core.Mappers;
using Ecommerce.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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

// Add API explorer services.
builder.Services.AddEndpointsApiExplorer();

// Add Swagger generation services to the dependency injection container to create swagger specification.
builder.Services.AddSwaggerGen();

// Add CORS policy to allow requests from the specified origin with any method and header.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    });
});

// Build the web application.
var app = builder.Build();

// Add custom exception handling middleware.
app.UseExceptionHandlingMiddleware();

// Add routing.
app.UseRouting();

// Adds endpoint that can serve the swagger.json file.
app.UseSwagger();

// Adds the swagger ui which can be used to test the API endpoints and view the API documentation.
app.UseSwaggerUI();

app.UseCors();

// Add authentication and authorization middleware.
app.UseAuthentication();
app.UseAuthorization();

// Map controller endpoints.
app.MapControllers();

app.Run();
