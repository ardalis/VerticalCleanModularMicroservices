using Ardalis.Result;
using Mediator;

namespace Nimble.Modulith.Customers.UseCases.Orders.Commands;

public record DeleteOrderItemCommand(int OrderId, int OrderItemId) : ICommand<Result<OrderDto>>;
