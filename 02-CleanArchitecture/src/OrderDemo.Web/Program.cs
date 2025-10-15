using Microsoft.EntityFrameworkCore;
using Mediator;
using OrderDemo.Infrastructure.Data;
using OrderDemo.Web.Products;
using OrderDemo.Web.Carts;
using OrderDemo.Web.Orders;
using Scalar.AspNetCore;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Register DbContext - use "AppDb" to match your AppHost configuration
builder.AddSqlServerDbContext<AppDbContext>("AppDb");

// Mediator
builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

builder.Services.AddOpenApi();

var app = builder.Build();

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

// Map endpoints
app.MapListProducts();
app.MapGetProductById();
app.MapAddToCart();
app.MapViewCart();
app.MapCheckout();
app.MapGetOrders();
app.MapConfirmPurchase();

app.MapGet("/", () => Results.Redirect("/scalar/v1"));

app.MapDefaultEndpoints(); // Important for Aspire health checks
app.Run();
