using Mediator;
using Microsoft.EntityFrameworkCore;
using OrderDemo.Api.Data;

namespace OrderDemo.Api.Features.Orders.GetOrders;

public record GetOrdersQuery() : IRequest<IReadOnlyList<OrderSummaryDto>>;

public record OrderSummaryDto(Guid Id, DateTime CreatedOn, decimal Total, int ItemCount);

public sealed class GetOrdersHandler(AppDbContext db)
    : IRequestHandler<GetOrdersQuery, IReadOnlyList<OrderSummaryDto>>
{
    public async ValueTask<IReadOnlyList<OrderSummaryDto>> Handle(GetOrdersQuery request, CancellationToken ct)
    {
        return await db.Orders
            .Include(o => o.Items)
            .Select(o => new OrderSummaryDto(
                o.Id,
                o.CreatedOn,
                o.Items.Sum(i => i.UnitPrice * i.Quantity),
                o.Items.Count))
            .OrderByDescending(o => o.CreatedOn)
            .ToListAsync(ct);
    }
}

public static class GetOrdersEndpoint
{
    public static void MapGetOrders(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/orders", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetOrdersQuery());
            return Results.Ok(result);
        })
        .WithName("GetOrders")
        .WithOpenApi();
    }
}
