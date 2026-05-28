using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nimble.Modulith.Customers.Domain.Interfaces;
using Nimble.Modulith.Customers.Infrastructure;
using Nimble.Modulith.Customers.Infrastructure.Data;
using Serilog;

namespace Nimble.Modulith.Customers;

public static class CustomersModuleExtensions
{
    public static IHostApplicationBuilder AddCustomersModuleServices(
        this IHostApplicationBuilder builder,
        ILogger logger)
    {
        // Add SQL Server DbContext with Aspire
        builder.AddSqlServerDbContext<CustomersDbContext>("customersdb");

        // Register repositories
        builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfReadRepository<>));

        // Register authorization service
        builder.Services.AddScoped<ICustomerAuthorizationService, CustomerAuthorizationService>();

        logger.Information("{Module} module services registered", nameof(CustomersModuleExtensions).Replace("ModuleExtensions", ""));

        return builder;
    }

    public static async Task<WebApplication> EnsureCustomersModuleDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CustomersDbContext>();
        await context.Database.MigrateAsync();
        
        return app;
    }
}
