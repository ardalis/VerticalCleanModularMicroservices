using OrderDemo.CleanArch.Core.OrderAggregate;

namespace OrderDemo.CleanArch.Core.ProductAggregate.Specifications;

public class OrderWithItemsSpec : Specification<Order>
{
  public OrderWithItemsSpec() =>
    Query
        .Include(order => order.Items);
}
