using Microsoft.EntityFrameworkCore;
using OrderDemo.Api.CartFeature;
using OrderDemo.Api.Data;
using OrderDemo.Api.OrderFeature;
using OrderDemo.Api.ProductFeature;
using Scalar.AspNetCore;
using Serilog;
using ServiceDefaults;

Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog();

// Add Aspire service defaults
builder.AddServiceDefaults();

// Register DbContext - use "AppDb" to match your AppHost configuration
builder.AddSqlServerDbContext<AppDbContext>("AppDb");

// Mediator
builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;

    options.PipelineBehaviors = new[]
    {
        typeof(LoggingBehavior<,>)
    };
});

builder.Services.AddOpenApi();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

var app = builder.Build();

// Apply migrations and seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Map endpoints
app.MapGetOrders();
app.MapListProductsEndpoint();
app.MapGetProductByIdEndpoint();
app.MapAddToCartEndpoint();
app.MapViewCartEndpoint();
app.MapCheckoutEndpoint();
app.MapConfirmPurchaseEndpoint();

app.MapGet("/", () => Results.Redirect("/docs"));

// Apply migrations on startup
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }

    app.MapOpenApi();
    app.MapScalarApiReference(); // /scalar/v1
}

app.MapDefaultEndpoints(); // Important for Aspire health checks
app.Run();

// Make the implicit Program class public for integration tests
public partial class Program { }
