using Mediator;
using Microsoft.EntityFrameworkCore;
using OrderDemo.Api.Data;
using OrderDemo.Api.Data.Entities;

namespace OrderDemo.Api.Features.Orders.CreateOrder;

// Request and Response DTOs
public record CreateOrderRequest(IReadOnlyList<CreateOrderItemDto> Items) : IRequest<CreateOrderResponse>;
public record CreateOrderItemDto(int ProductId, int Quantity);
public record CreateOrderResponse(Guid OrderId, decimal Total, int ItemCount);


public sealed class CreateOrderHandler(AppDbContext db)
    : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
{
    public async ValueTask<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken ct)
    {
        var productIds = request.Items.Select(i => i.ProductId).ToArray();

        var products = await db.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, ct);

        var order = new Order();

        foreach (var item in request.Items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
                throw new InvalidOperationException($"Invalid ProductId {item.ProductId}");

            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.UnitPrice
            });
        }

        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);

        return new CreateOrderResponse(order.Id, order.Total, order.Items.Count);
    }
}


public static class CreateOrderEndpoint
{
    public static void MapCreateOrder(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/orders", async (CreateOrderRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(request);
            return Results.Created($"/orders/{result.OrderId}", result);
        })
        .WithName("CreateOrder")
        .WithOpenApi();
    }
}
