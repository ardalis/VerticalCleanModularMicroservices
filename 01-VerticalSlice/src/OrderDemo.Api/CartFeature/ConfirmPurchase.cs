using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDemo.Api.Data;
using OrderDemo.Api.Services;

namespace OrderDemo.Api.CartFeature;

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

            if (paymentResult.StartsWith("Success"))
            {
                order.DatePaid = DateTimeOffset.UtcNow;
                order.PaymentReference = paymentResult.Split('=').Last();
                await dbContext.SaveChangesAsync();

                var response = new ConfirmPurchaseResponse { Message = "Purchase confirmed", OrderId = order.Id };
                return Results.Ok(response);
            }

            return Results.BadRequest(new ConfirmPurchaseResponse { Message = paymentResult });
        })
        .WithName("ConfirmPurchase")
        .WithTags("Cart");
    }

    public record ConfirmPurchaseRequest(Guid OrderId, string PaymentIntent);

    public record ConfirmPurchaseResponse
    {
        public string Message { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
    }
}