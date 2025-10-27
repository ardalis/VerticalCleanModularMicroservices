using Ardalis.Result;
using Mediator;

namespace Nimble.Modulith.Customers.UseCases.Orders.Commands;

public record CreateOrderCommand(
    int CustomerId,
    DateOnly OrderDate,
    List<CreateOrderItemDto> Items
) : ICommand<Result<OrderDto>>;

public record CreateOrderItemDto(
    int ProductId,
    int Quantity
);
