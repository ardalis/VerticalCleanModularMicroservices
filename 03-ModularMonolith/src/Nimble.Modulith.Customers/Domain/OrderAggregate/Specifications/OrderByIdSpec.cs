using Ardalis.Specification;

namespace Nimble.Modulith.Customers.Domain.OrderAggregate.Specifications;

public class OrderByIdSpec : Specification<Order>
{
    public OrderByIdSpec(int orderId)
    {
        Query.Where(o => o.Id == orderId)
            .Include(o => o.Items);
    }
}
