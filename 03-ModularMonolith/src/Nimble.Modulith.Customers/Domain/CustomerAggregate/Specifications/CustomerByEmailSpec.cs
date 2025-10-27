using Ardalis.Specification;

namespace Nimble.Modulith.Customers.Domain.CustomerAggregate.Specifications;

public class CustomerByEmailSpec : Specification<Customer>
{
    public CustomerByEmailSpec(string email)
    {
        Query.Where(c => c.Email == email);
    }
}
