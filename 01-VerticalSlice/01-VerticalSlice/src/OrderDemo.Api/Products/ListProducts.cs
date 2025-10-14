using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDemo.Api.Data;

namespace OrderDemo.Api.Products;

public static class ListProducts
{
    public static void MapListProductsEndpoint(this WebApplication app)
    {
        app.MapGet("/products", async (AppDbContext dbContext) =>
        {
            var products = await dbContext.Products
                .Select(p => new { p.Id, p.Name, p.UnitPrice })
                .ToListAsync();

            return Results.Ok(products);
        })
        .WithName("ListProducts")
        .WithTags("Products");
    }
}