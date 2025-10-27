using Ardalis.Specification;

namespace Nimble.Modulith.Customers.Domain.OrderAggregate;

public class OrderByIdSpec : Specification<Order>, ISingleResultSpecification<Order>
{
    public OrderByIdSpec(int orderId)
    {
        Query
            .Where(o => o.Id == orderId)
            .Include(o => o.Items);
    }
}
