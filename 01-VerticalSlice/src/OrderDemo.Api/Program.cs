using Microsoft.EntityFrameworkCore;
using Mediator;
using OrderDemo.Api.Data;
using OrderDemo.Api.Features.Orders.CreateOrder;
using OrderDemo.Api.Features.Orders.GetOrders;
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

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations and seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Map endpoints
app.MapCreateOrder();
app.MapGetOrders();

app.MapGet("/", () => Results.Redirect("/swagger"));

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
}

app.MapDefaultEndpoints(); // Important for Aspire health checks
app.Run();
