using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Add ocelot configuration file.
builder.Configuration.AddJsonFile("ocelot.json", false, true);

// Add ocelot services to the container.
builder.Services.AddOcelot();

app.Run();

// Use ocelot middleware to handle incoming requests and route them to the appropriate downstream services.
// This is asynchronous so await is used to ensure that the application waits for the middleware to be fully set up before processing requests.
await app.UseOcelot();
