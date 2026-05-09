using Ecommerce.API.Handlers;
using Ecommerce.BusinessLogicLayer;
using Ecommerce.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);

// Add exception handling middleware.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add data access layer services.
builder.Services.AddDataAccessLayer();

// Add business logic layer services.
builder.Services.AddBusinessLogicLayer();

var app = builder.Build();

app.UseExceptionHandler();

app.UseRouting();

app.Run();
