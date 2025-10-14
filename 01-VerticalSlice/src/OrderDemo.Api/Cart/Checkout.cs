using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDemo.Api.Data;
using OrderDemo.Api.Data.Entities;

namespace OrderDemo.Api.Cart;

public static class Checkout
{
    public static void MapCheckoutEndpoint(this WebApplication app)
    {
        app.MapPost("/checkout", async (CheckoutRequest request, AppDbContext dbContext) =>
        {
            var order = await dbContext.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId);

            if (order is null)
            {
                return Results.NotFound("Order not found");
            }

            var guestUser = await dbContext.GuestUsers
                .FirstOrDefaultAsync(g => g.Email == request.Email);

            if (guestUser is null)
            {
                guestUser = new GuestUser { Id = Guid.NewGuid(), Email = request.Email };
                dbContext.GuestUsers.Add(guestUser);
            }

            order.GuestUserId = guestUser.Id;

            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Message = "Checkout successful", OrderId = order.Id });
        })
        .WithName("Checkout")
        .WithTags("Cart");
    }

    public record CheckoutRequest(Guid OrderId, string Email);
}