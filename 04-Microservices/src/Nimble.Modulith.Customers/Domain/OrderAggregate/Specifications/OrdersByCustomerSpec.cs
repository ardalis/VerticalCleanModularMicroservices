using Ardalis.Specification;

namespace Nimble.Modulith.Customers.Domain.OrderAggregate.Specifications;

public class OrdersByCustomerSpec : Specification<Order>
{
    public OrdersByCustomerSpec(int customerId)
    {
        Query.Where(o => o.CustomerId == customerId)
            .Include(o => o.Items)
            .OrderByDescending(o => o.OrderDate);
    }
}
