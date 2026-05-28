using Ardalis.Result;
using Mediator;
using Nimble.Modulith.Customers.Domain.Interfaces;
using Nimble.Modulith.Customers.Domain.OrderAggregate;

namespace Nimble.Modulith.Customers.UseCases.Orders.Queries;

public class ListOrdersByDateHandler(IReadRepository<Order> repository) 
    : IQueryHandler<ListOrdersByDateQuery, Result<List<OrderDto>>>
{
    public async ValueTask<Result<List<OrderDto>>> Handle(ListOrdersByDateQuery query, CancellationToken ct)
    {
        var spec = new OrdersByDateSpec(query.OrderDate);
        var orders = await repository.ListAsync(spec, ct);

        var dtos = orders.Select(order => new OrderDto(
            order.Id,
            order.CustomerId,
            order.OrderNumber,
            order.OrderDate,
            order.Status.ToString(),
            order.TotalAmount,
            order.Items.Select(i => new OrderItemDto(
                i.Id,
                i.ProductId,
                i.ProductName,
                i.Quantity,
                i.UnitPrice,
                i.TotalPrice
            )).ToList(),
            order.CreatedAt,
            order.UpdatedAt
        )).ToList();

        return Result<List<OrderDto>>.Success(dtos);
    }
}
