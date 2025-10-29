using OrderDemo.CleanArch.Core.OrderAggregate;
using OrderDemo.CleanArch.Core.ProductAggregate.Specifications;
using OrderDemo.CleanArch.UseCases.Cart;
using OrderDemo.CleanArch.UseCases.Cart.Get;

namespace OrderDemo.CleanArch.UseCases.Orders.List;

public record ListOrdersQuery : IQuery<Result<List<OrderDto>>>;

public class ListOrdersQueryHandler(IReadRepository<Order> repository)
  : IQueryHandler<ListOrdersQuery, Result<List<OrderDto>>>
{
  private readonly IReadRepository<Order> _repository = repository;

  public async ValueTask<Result<List<OrderDto>>> Handle(ListOrdersQuery query, CancellationToken cancellationToken)
  {
    var spec = new OrderWithItemsSpec();
    var orders = (await _repository.ListAsync(spec, cancellationToken: cancellationToken))
        .Select(o => new OrderDto
        {
          Id = o.Id.Value.ToString(),
          OrderDate = o.CreatedOn.Date,
          TotalAmount = o.Total
        })
        .ToList();
    return orders;
  }
}
