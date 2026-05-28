using Ardalis.Result;
using Mediator;

namespace Nimble.Modulith.Customers.UseCases.Customers.Queries;

public record ListCustomersQuery() : IQuery<Result<List<CustomerDto>>>;
