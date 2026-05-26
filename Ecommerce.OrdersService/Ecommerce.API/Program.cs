using Ecommerce.API.Handlers;
using Ecommerce.BusinessLogicLayer;
using Ecommerce.BusinessLogicLayer.HttpClients;
using Ecommerce.DataAccessLayer;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger generation services to the dependency injection container to create swagger specification.
builder.Services.AddSwaggerGen();

// Add business logic layer services.
builder.Services.AddBusinessLogicLayer();

// Add data access layer services.
builder.Services.AddDataAccessLayer(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add CORS policy to allow requests from the specified origin with any method and header.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();

builder.Services.AddHttpClient<UsersMicroserviceClient>(config =>
{
    config.BaseAddress = new Uri($"http://{builder.Configuration["UsersMicroserviceName"]}:{builder.Configuration["UsersMicroservicePort"]}");
})
// Add retry policy using Polly to handle transient faults when calling the users microservice.
// The policy will retry the request up to 5 times with a delay of 3 seconds between each retry if the response is not successful.
.AddPolicyHandler(Policy.HandleResult<HttpResponseMessage>(result => !result.IsSuccessStatusCode)
.WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(3), onRetry: (response, timespan, retryCount, context) =>
{
    // TODO: Add logging here to log the failed response and retry attempt.
}));

builder.Services.AddHttpClient<ProductsMicroserviceClient>(config =>
{
    config.BaseAddress = new Uri($"http://{builder.Configuration["ProductsMicroserviceName"]}:{builder.Configuration["ProductsMicroservicePort"]}");
});

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    // Adds endpoint that can serve the swagger.json file.
    app.UseSwagger();

    // Adds the swagger ui which can be used to test the API endpoints and view the API documentation.
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
