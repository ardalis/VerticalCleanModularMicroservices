using Mediator;
using OrderDemo.UseCases.Products;

namespace OrderDemo.Web.Products;

public static class ListProductsEndpoint
{
    public static void MapListProducts(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/products", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new ListProductsQuery());
            return Results.Ok(result);
        })
        .WithName("ListProducts")
        .WithTags("Products")
        .WithOpenApi();
    }
}
