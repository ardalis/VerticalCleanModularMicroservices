using Mediator;
using OrderDemo.UseCases.Products;

namespace OrderDemo.Web.Products;

public static class GetProductByIdEndpoint
{
    public static void MapGetProductById(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/products/{id}", async (int id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetProductByIdQuery(id));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetProductById")
        .WithTags("Products")
        .WithOpenApi();
    }
}
