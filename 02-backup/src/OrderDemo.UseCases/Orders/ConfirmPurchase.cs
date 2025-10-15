using Mediator;
using Microsoft.EntityFrameworkCore;
using OrderDemo.Infrastructure.Data;

namespace OrderDemo.UseCases.Orders;

public record ConfirmPurchaseCommand(Guid OrderId, string PaymentIntent) : ICommand<ConfirmPurchaseResultDto>;

public record ConfirmPurchaseResultDto(bool Success, string Message, Guid OrderId);

public sealed class ConfirmPurchaseHandler(AppDbContext dbContext)
    : ICommandHandler<ConfirmPurchaseCommand, ConfirmPurchaseResultDto>
{
    public async ValueTask<ConfirmPurchaseResultDto> Handle(ConfirmPurchaseCommand command, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);

        if (order is null)
        {
            throw new InvalidOperationException("Order not found");
        }

        var totalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice);
        
        // TODO: Inject payment service as dependency
        var paymentResult = ProcessPayment(totalAmount, "demo-merchant-key", command.PaymentIntent);

        if (paymentResult.StartsWith("Success"))
        {
            order.DatePaid = DateTimeOffset.UtcNow;
            order.PaymentReference = paymentResult.Split('=').Last();
            await dbContext.SaveChangesAsync(cancellationToken);

            return new ConfirmPurchaseResultDto(true, "Purchase confirmed", order.Id);
        }

        return new ConfirmPurchaseResultDto(false, paymentResult, order.Id);
    }

    // Temporary - should be extracted to a service
    private static string ProcessPayment(decimal amount, string merchantKey, string paymentIntent)
    {
        if (string.IsNullOrEmpty(paymentIntent))
        {
            return "Error: Payment intent is required";
        }

        if (amount <= 0)
        {
            return "Error: Invalid amount";
        }

        var referenceNumber = $"REF-{Guid.NewGuid():N}";
        return $"Success: Payment processed. Reference={referenceNumber}";
    }
}
