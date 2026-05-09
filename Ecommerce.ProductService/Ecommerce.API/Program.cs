using Ecommerce.BusinessLogicLayer;
using Ecommerce.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);

// Add data access layer services.
builder.Services.AddDataAccessLayer();

// Add business logic layer services.
builder.Services.AddBusinessLogicLayer();

var app = builder.Build();

app.Run();
