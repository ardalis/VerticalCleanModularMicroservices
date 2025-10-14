using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDemo.Api.Data;
using OrderDemo.Api.Data.Entities;

namespace OrderDemo.Api.Cart;

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

            var order = await dbContext.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId);

            if (order is null)
            {
                order = new Order
                {
                    Id = Guid.NewGuid()
                };
                dbContext.Orders.Add(order);
            }

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = request.Quantity,
                UnitPrice = product.UnitPrice
            });

            await dbContext.SaveChangesAsync();

            return Results.Ok(order);
        })
        .WithName("AddToCart")
        .WithTags("Cart");
    }

    public record AddToCartRequest(Guid OrderId, int ProductId, int Quantity);
}