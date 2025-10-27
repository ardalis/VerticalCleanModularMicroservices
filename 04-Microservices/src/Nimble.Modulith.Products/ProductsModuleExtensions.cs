using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nimble.Modulith.Products.Data;
using Serilog;

namespace Nimble.Modulith.Products;

public static class ProductsModuleExtensions
{
    public static IHostApplicationBuilder AddProductsModuleServices(
        this IHostApplicationBuilder builder,
        ILogger logger)
    {
        // Add SQL Server DbContext with Aspire
        builder.AddSqlServerDbContext<ProductsDbContext>("productsdb");

        logger.Information("{Module} module services registered", nameof(ProductsModuleExtensions).Replace("ModuleExtensions", ""));

        return builder;
    }

    public static async Task<WebApplication> EnsureProductsModuleDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
        await context.Database.MigrateAsync();
        
        return app;
    }
}
