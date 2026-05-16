using Ecommerce.BusinessLogicLayer;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger generation services to the dependency injection container to create swagger specification.
builder.Services.AddSwaggerGen();

// Add business logic layer services.
builder.Services.AddBusinessLogicLayer();

var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    // Adds endpoint that can serve the swagger.json file.
    app.UseSwagger();

    // Adds the swagger ui which can be used to test the API endpoints and view the API documentation.
    app.UseSwaggerUI();
}

app.Run();
