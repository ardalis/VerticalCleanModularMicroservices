using Ardalis.Specification;

namespace Nimble.Modulith.Customers.Domain.CustomerAggregate.Specifications;

public class CustomerByIdSpec : Specification<Customer>
{
    public CustomerByIdSpec(int customerId)
    {
        Query.Where(c => c.Id == customerId);
    }
}
