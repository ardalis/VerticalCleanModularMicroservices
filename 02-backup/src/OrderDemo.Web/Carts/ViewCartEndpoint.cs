using Mediator;
using OrderDemo.UseCases.Carts;

namespace OrderDemo.Web.Carts;

public static class ViewCartEndpoint
{
    public static void MapViewCart(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/cart/{cartId}", async (Guid cartId, IMediator mediator) =>
        {
            var result = await mediator.Send(new ViewCartQuery(cartId));
            return result is not null ? Results.Ok(result) : Results.NotFound("Cart not found");
        })
        .WithName("ViewCart")
        .WithTags("Cart")
        .WithOpenApi();
    }
}
