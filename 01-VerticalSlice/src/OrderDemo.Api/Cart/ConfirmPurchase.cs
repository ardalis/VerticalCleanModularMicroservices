using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDemo.Api.Data;
using OrderDemo.Api.Services;

namespace OrderDemo.Api.Cart;

public static class ConfirmPurchase
{
    public static void MapConfirmPurchaseEndpoint(this WebApplication app)
    {
        app.MapPost("/confirm-purchase", async (ConfirmPurchaseRequest request, AppDbContext dbContext) =>
        {
            var order = await dbContext.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId);

            if (order is null)
            {
                return Results.NotFound("Order not found");
            }

            var totalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice);
            var paymentResult = PaymentApi.ProcessPayment(totalAmount, "demo-merchant-key", request.PaymentIntent);

            if (paymentResult == "Success")
            {
                return Results.Ok(new { Message = "Purchase confirmed", OrderId = order.Id });
            }

            return Results.BadRequest(new { Message = paymentResult });
        })
        .WithName("ConfirmPurchase")
        .WithTags("Cart");
    }

    public record ConfirmPurchaseRequest(Guid OrderId, string PaymentIntent);
}