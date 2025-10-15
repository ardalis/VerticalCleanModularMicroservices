using Mediator;
using OrderDemo.UseCases.Orders;

namespace OrderDemo.Web.Orders;

public static class GetOrdersEndpoint
{
    public static void MapGetOrders(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/orders", async (string emailAddress, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetOrdersQuery(emailAddress));
            return Results.Ok(result);
        })
        .WithName("GetOrders")
        .WithTags("Orders")
        .WithOpenApi();
    }
}
