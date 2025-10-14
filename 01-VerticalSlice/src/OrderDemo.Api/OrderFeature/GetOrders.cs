using Mediator;
using Microsoft.EntityFrameworkCore;
using OrderDemo.Api.Data;

namespace OrderDemo.Api.OrderFeature;

public record GetOrdersQuery(string emailAddress) : IRequest<IReadOnlyList<OrderSummaryDto>>;

public record OrderSummaryDto(Guid Id, string CreatedOn, decimal Total, int ItemCount, string Status, string DatePaid, string PaymentReference);

public sealed class GetOrdersHandler(AppDbContext db)
    : IRequestHandler<GetOrdersQuery, IReadOnlyList<OrderSummaryDto>>
{
    public async ValueTask<IReadOnlyList<OrderSummaryDto>> Handle(GetOrdersQuery request, CancellationToken ct)
    {
        return await db.Orders
            .Include(o => o.Items)
            .Include(o => o.GuestUser)
            .Where(o => o.GuestUser != null && o.GuestUser.Email == request.emailAddress)
            .OrderByDescending(o => o.CreatedOn)
            .Select(o => new OrderSummaryDto(
                o.Id,
                o.CreatedOn.ToString(),
                o.Items.Sum(i => i.UnitPrice * i.Quantity),
                o.Items.Count,
                o.DatePaid.HasValue ? "Paid" : "Pending Payment",
                o.DatePaid.HasValue ? o.DatePaid.ToString() : "N/A",
                o.PaymentReference
            ))
            .ToListAsync(ct);
    }
}

public static class GetOrdersEndpoint
{
    public static void MapGetOrders(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/orders", async (IMediator mediator, string emailAddress) =>
        {
            var result = await mediator.Send(new GetOrdersQuery(emailAddress));
            return Results.Ok(result);
        })
        .WithName("GetOrders")
        .WithOpenApi();
    }
}
