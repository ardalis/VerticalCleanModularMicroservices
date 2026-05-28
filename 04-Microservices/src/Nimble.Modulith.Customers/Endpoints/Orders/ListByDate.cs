using FastEndpoints;
using Mediator;
using Nimble.Modulith.Customers.UseCases.Orders.Queries;

namespace Nimble.Modulith.Customers.Endpoints.Orders;

public class ListByDate(IMediator mediator) : EndpointWithoutRequest<List<OrderResponse>>
{
    public override void Configure()
    {
        Get("/orders/by-date/{date}");
        Roles("Admin"); // Only admins can list all orders
        Summary(s =>
        {
            s.Summary = "List orders by date";
            s.Description = "Returns all orders created on the specified date (Admin only)";
        });
        Tags("orders");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var dateString = Route<string>("date");
        if (!DateOnly.TryParse(dateString, out var date))
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var query = new ListOrdersByDateQuery(date);
        var result = await mediator.Send(query, ct);

        if (!result.IsSuccess)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Response = result.Value.Select(o => new OrderResponse(
            o.Id,
            o.CustomerId,
            o.OrderNumber,
            o.OrderDate,
            o.Status,
            o.TotalAmount,
            o.Items.Select(i => new OrderItemResponse(
                i.Id,
                i.ProductId,
                i.ProductName,
                i.Quantity,
                i.UnitPrice,
                i.TotalPrice
            )).ToList()
        )).ToList();
    }
}
