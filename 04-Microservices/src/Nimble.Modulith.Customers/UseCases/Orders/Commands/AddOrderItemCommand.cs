using Ardalis.Result;
using Mediator;

namespace Nimble.Modulith.Customers.UseCases.Orders.Commands;

public record AddOrderItemCommand(
    int OrderId,
    int ProductId,
    int Quantity
) : ICommand<Result<OrderDto>>;
