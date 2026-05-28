using OrderDemo.CleanArch.Core.ProductAggregate;
using OrderDemo.CleanArch.Core.ProductAggregate.Specifications;

namespace OrderDemo.CleanArch.UseCases.Products.Get;

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
