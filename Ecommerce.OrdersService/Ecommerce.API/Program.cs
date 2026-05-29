using Ecommerce.API.Handlers;
using Ecommerce.BusinessLogicLayer;
using Ecommerce.BusinessLogicLayer.HttpClients;
using Ecommerce.BusinessLogicLayer.Policies.Contracts;
using Ecommerce.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger generation services to the dependency injection container to create swagger specification.
builder.Services.AddSwaggerGen();

// Add business logic layer services.
builder.Services.AddBusinessLogicLayer(builder.Configuration);

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

#pragma warning disable ASP0000
var sp = builder.Services.BuildServiceProvider();
#pragma warning restore ASP0000
var usersMicroservicePolicies = sp.GetRequiredService<IUserMicroservicePolicies>();
var productsMicroservicePolicies = sp.GetRequiredService<IProductsMicroservicePolicies>();

builder.Services.AddHttpClient<UsersMicroserviceClient>(config =>
{
    config.BaseAddress = new Uri($"http://{builder.Configuration["UsersMicroserviceName"]}:{builder.Configuration["UsersMicroservicePort"]}");
})
// Add retry policy using Polly to handle transient faults when calling the users microservice.
.AddPolicyHandler(usersMicroservicePolicies.GetRetryPolicy())
// Add circuit breaker policy handler using Polly to the user micorservice client.
.AddPolicyHandler(usersMicroservicePolicies.GetCircuitBreakerPolicy())
// Add timeout policy handler using Polly to the user micorservice client.
.AddPolicyHandler(usersMicroservicePolicies.GetTimeoutPolicy());

builder.Services.AddHttpClient<ProductsMicroserviceClient>(config =>
{
    config.BaseAddress = new Uri($"http://{builder.Configuration["ProductsMicroserviceName"]}:{builder.Configuration["ProductsMicroservicePort"]}");
})
// Add fallback policy handler using Polly to the products micorservice client.
.AddPolicyHandler(productsMicroservicePolicies.GetFallbackPolicy())
// Add bulkhead isolation policy handler using Polly to the products micorservice client.
.AddPolicyHandler(productsMicroservicePolicies.GetBulkheadIsolationPolicy());

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
