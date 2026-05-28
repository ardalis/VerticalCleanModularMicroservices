using Ardalis.Result;
using Mediator;

namespace Nimble.Modulith.Customers.UseCases.Customers.Queries;

public record GetCustomerByIdQuery(int Id) : IQuery<Result<CustomerDto>>;
