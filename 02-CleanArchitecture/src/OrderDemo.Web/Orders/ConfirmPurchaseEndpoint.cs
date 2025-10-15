using Mediator;
using OrderDemo.UseCases.Orders;

namespace OrderDemo.Web.Orders;

public static class ConfirmPurchaseEndpoint
{
    public static void MapConfirmPurchase(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/confirm-purchase", async (ConfirmPurchaseRequest request, IMediator mediator) =>
        {
            try
            {
                var command = new ConfirmPurchaseCommand(request.OrderId, request.PaymentIntent);
                var result = await mediator.Send(command);
                
                if (result.Success)
                {
                    return Results.Ok(result);
                }
                
                return Results.BadRequest(result);
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(ex.Message);
            }
        })
        .WithName("ConfirmPurchase")
        .WithTags("Orders")
        .WithOpenApi();
    }

    public record ConfirmPurchaseRequest(Guid OrderId, string PaymentIntent);
}
