using Ardalis.Result;
using Mediator;

namespace Nimble.Modulith.Customers.UseCases.Orders.Commands;

public record ConfirmOrderCommand(int OrderId) : ICommand<Result<OrderDto>>;
