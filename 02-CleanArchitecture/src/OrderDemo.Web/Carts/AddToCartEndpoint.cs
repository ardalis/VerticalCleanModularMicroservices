using Mediator;
using OrderDemo.UseCases.Carts;

namespace OrderDemo.Web.Carts;

public static class AddToCartEndpoint
{
    public static void MapAddToCart(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/cart", async (AddToCartRequest request, IMediator mediator) =>
        {
            try
            {
                var command = new AddToCartCommand(request.CartId, request.ProductId, request.Quantity);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(ex.Message);
            }
        })
        .WithName("AddToCart")
        .WithTags("Cart")
        .WithOpenApi();
    }

    public record AddToCartRequest(Guid CartId, int ProductId, int Quantity);
}
