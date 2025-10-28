using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Mediator;
using Nimble.Modulith.Customers;
using Nimble.Modulith.Products;
using Nimble.Modulith.Reporting;
using Nimble.Modulith.SharedInfrastructure;
using Nimble.Modulith.Users;
using Nimble.Modulith.Web;
using Serilog;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (Aspire configuration)
builder.AddServiceDefaults();

// Add Mediator with source generation
builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

// Add logging behavior to Mediator pipeline
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>)); // universal

// Add shared messaging infrastructure (MassTransit + RabbitMQ)
builder.AddSharedMessagingInfrastructure();

// Add FastEndpoints with JWT Bearer Authentication and Authorization
builder.Services.AddFastEndpoints()
    .AddAuthenticationJwtBearer(s =>
    {
        s.SigningKey = builder.Configuration["Auth:JwtSecret"];
    })
    .AddAuthorization()
    .SwaggerDocument();

// Add module services
builder.AddUsersModuleServices(logger);
builder.AddProductsModuleServices(logger);
builder.AddCustomersModuleServices(logger);
builder.AddReportingModuleServices(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints()
    .UseSwaggerGen();

// Ensure module databases are created
await app.EnsureUsersModuleDatabaseAsync();
await app.EnsureProductsModuleDatabaseAsync();
await app.EnsureCustomersModuleDatabaseAsync();
await app.EnsureReportingModuleDatabaseAsync();

app.Run();


