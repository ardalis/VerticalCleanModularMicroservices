using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDemo.Api.Data;

namespace OrderDemo.Api.Products;

public static class GetProductById
{
    public static void MapGetProductByIdEndpoint(this WebApplication app)
    {
        app.MapGet("/products/{id}", async (int id, AppDbContext dbContext) =>
        {
            var product = await dbContext.Products
                .Where(p => p.Id == id)
                .Select(p => new { p.Id, p.Name, p.UnitPrice })
                .FirstOrDefaultAsync();

            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .WithTags("Products");
    }
}