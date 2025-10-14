using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDemo.Api.Data;
using OrderDemo.Api.Data.Entities;

namespace OrderDemo.Api.CartFeature;

public static class Checkout
{
    public static void MapCheckoutEndpoint(this WebApplication app)
    {
        app.MapPost("/checkout", async (CheckoutRequest request, AppDbContext dbContext) =>
        {
            var cart = await dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == request.CartId && !c.Deleted);

            if (cart is null)
            {
                return Results.NotFound("Cart not found");
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                GuestUserId = request.GuestUserId
            };

            order.Items.AddRange(cart.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }));

            dbContext.Orders.Add(order);
            cart.Deleted = true;

            await dbContext.SaveChangesAsync();

            var response = new CheckoutResponse { Message = "Checkout successful", OrderId = order.Id };
            return Results.Ok(response);
        })
        .WithName("Checkout")
        .WithTags("Cart");
    }

    public record CheckoutRequest(Guid CartId, Guid GuestUserId);

    public record CheckoutResponse
    {
        public string Message { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
    }
}