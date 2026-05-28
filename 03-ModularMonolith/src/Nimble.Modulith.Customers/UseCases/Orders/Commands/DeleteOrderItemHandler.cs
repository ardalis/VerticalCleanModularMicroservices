using Ardalis.Result;
using Mediator;
using Nimble.Modulith.Customers.Domain.Interfaces;
using Nimble.Modulith.Customers.Domain.OrderAggregate;

namespace Nimble.Modulith.Customers.UseCases.Orders.Commands;

public class DeleteOrderItemHandler(IRepository<Order> repository) 
    : ICommandHandler<DeleteOrderItemCommand, Result<OrderDto>>
{
    public async ValueTask<Result<OrderDto>> Handle(DeleteOrderItemCommand command, CancellationToken ct)
    {
        var order = await repository.GetByIdAsync(command.OrderId, ct);

        if (order is null)
        {
            return Result<OrderDto>.NotFound("Order not found");
        }

        var item = order.Items.FirstOrDefault(i => i.Id == command.OrderItemId);
        if (item is null)
        {
            return Result<OrderDto>.NotFound("Order item not found");
        }

        try
        {
            order.RemoveItem(item);
        }
        catch (InvalidOperationException ex)
        {
            return Result<OrderDto>.Error(ex.Message);
        }
        
        order.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(order, ct);
        await repository.SaveChangesAsync(ct);

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
