using Ardalis.Result;
using Mediator;

namespace Nimble.Modulith.Customers.UseCases.Orders.Queries;

public record ListOrdersByDateQuery(DateOnly OrderDate) : IQuery<Result<List<OrderDto>>>;
