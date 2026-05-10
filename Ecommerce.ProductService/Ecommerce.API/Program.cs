var builder = WebApplication.CreateBuilder(args);

// Add exception handling middleware.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add data access layer services.
builder.Services.AddDataAccessLayer(builder.Configuration);

// Add business logic layer services.
builder.Services.AddBusinessLogicLayer();

// Add Swagger generation services to the dependency injection container to create swagger specification.
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

app.UseRouting();

// Adds endpoint that can serve the swagger.json file.
app.UseSwagger();

// Adds the swagger ui which can be used to test the API endpoints and view the API documentation.
app.UseSwaggerUI();

// Map product api endpoints to the application.
app.MapProductAPIEndpoints();

app.Run();
