using OrderDemo.CleanVertical.Web.Domain.ProductAggregate;
using OrderDemo.CleanVertical.Web.Domain.ProductAggregate.Specifications;

namespace OrderDemo.CleanVertical.Web.ProductFeatures.GetById;

public record GetProductQuery(ProductId ProductId) : IQuery<Result<ProductDto>>;

public class GetProductHandler(IReadRepository<Product> _repository)
  : IQueryHandler<GetProductQuery, Result<ProductDto>>
{
  public async ValueTask<Result<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
  {
    var spec = new ProductByIdSpec(request.ProductId);
    var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
    if (entity == null) return Result.NotFound();

    return new ProductDto(entity.Id, entity.Name, entity.UnitPrice);
  }
}
