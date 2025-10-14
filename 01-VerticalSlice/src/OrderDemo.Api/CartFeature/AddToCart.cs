using Microsoft.EntityFrameworkCore;
using OrderDemo.Api.Data;
using OrderDemo.Api.Data.Entities;

namespace OrderDemo.Api.CartFeature;

public static class AddToCart
{
    public static void MapAddToCartEndpoint(this WebApplication app)
    {
        app.MapPost("/cart", async (AddToCartRequest request, AppDbContext dbContext) =>
        {
            var product = await dbContext.Products.FindAsync(request.ProductId);
            if (product is null)
            {
                return Results.NotFound("Product not found");
            }

            var cart = await dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == request.CartId && !c.Deleted);

            if (cart is null)
            {
                cart = new Data.Entities.Cart { Id = Guid.NewGuid() };
                dbContext.Carts.Add(cart);
            }

            cart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                Quantity = request.Quantity,
                UnitPrice = product.UnitPrice
            });

            await dbContext.SaveChangesAsync();

            var response = new CartDto
            {
                Id = cart.Id,
                Items = cart.Items.Select(i => new CartItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return Results.Ok(response);
        })
        .WithName("AddToCart")
        .WithTags("Cart");
    }

    public record AddToCartRequest(Guid CartId, int ProductId, int Quantity);

    public record CartDto
    {
        public Guid Id { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
    }

    public record CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}