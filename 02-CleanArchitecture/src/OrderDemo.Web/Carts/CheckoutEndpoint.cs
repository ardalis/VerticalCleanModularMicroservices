using Mediator;
using OrderDemo.UseCases.Carts;

namespace OrderDemo.Web.Carts;

public static class CheckoutEndpoint
{
    public static void MapCheckout(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/checkout", async (CheckoutRequest request, IMediator mediator) =>
        {
            try
            {
                var command = new CheckoutCommand(request.CartId, request.GuestUserId, request.Email);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(ex.Message);
            }
        })
        .WithName("Checkout")
        .WithTags("Cart")
        .WithOpenApi();
    }

    public record CheckoutRequest(Guid CartId, Guid GuestUserId, string Email);
}
