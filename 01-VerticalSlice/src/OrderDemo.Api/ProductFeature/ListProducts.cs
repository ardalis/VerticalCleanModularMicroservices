using Microsoft.EntityFrameworkCore;
using OrderDemo.Api.Data;

namespace OrderDemo.Api.ProductFeature;

public static class ListProducts
{
    public static void MapListProductsEndpoint(this WebApplication app)
    {
        app.MapGet("/products", async (AppDbContext dbContext) =>
        {
            var products = await dbContext.Products
                .Select(p => new ProductDto 
                    { Id = p.Id, Name = p.Name, UnitPrice = p.UnitPrice })
                .ToListAsync();

            return Results.Ok(products);
        })
        .WithName("ListProducts")
        .WithTags("Products");
    }

    public record ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
}