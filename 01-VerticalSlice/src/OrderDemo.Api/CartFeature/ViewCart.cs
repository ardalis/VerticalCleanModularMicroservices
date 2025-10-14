using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDemo.Api.Data;

namespace OrderDemo.Api.CartFeature;

public static class ViewCart
{
    public static void MapViewCartEndpoint(this WebApplication app)
    {
        app.MapGet("/cart/{cartId}", async (Guid cartId, AppDbContext dbContext) =>
        {
            var cart = await dbContext.Carts
                .Include(c => c.Items)
                .Where(c => c.Id == cartId && !c.Deleted)
                .Select(c => new CartDto
                {
                    Id = c.Id,
                    Items = c.Items.Select(i => new CartItemDto
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        TotalPrice = i.Quantity * i.UnitPrice
                    }).ToList(),
                    Total = c.Items.Sum(i => i.Quantity * i.UnitPrice)
                })
                .FirstOrDefaultAsync();

            return cart is not null ? Results.Ok(cart) : Results.NotFound("Cart not found");
        })
        .WithName("ViewCart")
        .WithTags("Cart");
    }

    public record CartDto
    {
        public Guid Id { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal Total { get; set; }
    }

    public record CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}