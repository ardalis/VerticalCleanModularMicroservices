using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDemo.Api.Data;

namespace OrderDemo.Api.ProductFeature;

public static class GetProductById
{
    public static void MapGetProductByIdEndpoint(this WebApplication app)
    {
        app.MapGet("/products/{id}", async (int id, AppDbContext dbContext) =>
        {
            var product = await dbContext.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductDto { Id = p.Id, Name = p.Name, UnitPrice = p.UnitPrice })
                .FirstOrDefaultAsync();

            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .WithTags("Products");
    }

    public record ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
    }
}