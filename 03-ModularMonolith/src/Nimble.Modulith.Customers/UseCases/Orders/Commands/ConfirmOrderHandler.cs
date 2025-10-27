using Ardalis.Result;
using Mediator;
using Nimble.Modulith.Customers.Contracts;
using Nimble.Modulith.Customers.Domain.Interfaces;
using Nimble.Modulith.Customers.Domain.OrderAggregate;
using Nimble.Modulith.Customers.UseCases.Customers.Queries;

namespace Nimble.Modulith.Customers.UseCases.Orders.Commands;

public class ConfirmOrderHandler(IRepository<Order> repository, IMediator mediator) 
    : ICommandHandler<ConfirmOrderCommand, Result<OrderDto>>
{
    public async ValueTask<Result<OrderDto>> Handle(ConfirmOrderCommand command, CancellationToken ct)
    {
        var order = await repository.GetByIdAsync(command.OrderId, ct);

        if (order is null)
        {
            return Result<OrderDto>.NotFound($"Order with ID {command.OrderId} not found");
        }

        if (order.Items.Count == 0)
        {
            return Result<OrderDto>.Error("Cannot confirm an order with no items");
        }

        // Get customer details for the event
        var customerQuery = new GetCustomerByIdQuery(order.CustomerId);
        var customerResult = await mediator.Send(customerQuery, ct);
        
        if (!customerResult.IsSuccess)
        {
            return Result<OrderDto>.Error($"Customer with ID {order.CustomerId} not found");
        }
        
        var customer = customerResult.Value;

        // Change order status to Confirmed
        order.Status = OrderStatus.Confirmed;
        order.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(order, ct);
        await repository.SaveChangesAsync(ct);

        // Publish OrderCreatedEvent for reporting
        var orderCreatedEvent = new OrderCreatedEvent(
            order.Id,
            order.CustomerId,
            customer.Email,
            order.OrderNumber,
            order.OrderDate,
            order.TotalAmount,
            order.Items.Select(i => new OrderItemDetails(
                i.Id,
                i.ProductId,
                i.ProductName,
                i.Quantity,
                i.UnitPrice,
                i.TotalPrice
            )).ToList()
        );

        await mediator.Publish(orderCreatedEvent, ct);

        var dto = new OrderDto(
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
        );

        return Result<OrderDto>.Success(dto);
    }
}
