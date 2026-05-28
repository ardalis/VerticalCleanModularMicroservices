using Ardalis.Result;
using Mediator;
using Nimble.Modulith.Customers.Domain.CustomerAggregate;
using Nimble.Modulith.Customers.Domain.Interfaces;

namespace Nimble.Modulith.Customers.UseCases.Customers.Queries;

public class ListCustomersHandler(IReadRepository<Customer> repository) 
    : IQueryHandler<ListCustomersQuery, Result<List<CustomerDto>>>
{
    public async ValueTask<Result<List<CustomerDto>>> Handle(ListCustomersQuery query, CancellationToken ct)
    {
        var customers = await repository.ListAsync(ct);
        
        var dtos = customers.Select(c => new CustomerDto(
            c.Id,
            c.FirstName,
            c.LastName,
            c.Email,
            c.PhoneNumber,
            new AddressDto(
                c.Address.Street,
                c.Address.City,
                c.Address.State,
                c.Address.PostalCode,
                c.Address.Country
            ),
            c.CreatedAt,
            c.UpdatedAt
        )).ToList();

        return Result<List<CustomerDto>>.Success(dtos);
    }
}
