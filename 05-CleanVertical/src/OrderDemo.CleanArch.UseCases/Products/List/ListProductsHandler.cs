using OrderDemo.CleanArch.UseCases.Products;
using OrderDemo.CleanArch.Web.ProductFeatures;

namespace OrderDemo.CleanVertical.UseCases.Products.List;

public class ListProductsHandler : IQueryHandler<ListProductsQuery, Result<PagedResult<ProductDto>>>
{
  private readonly IListProductsQueryService _query;

  public ListProductsHandler(IListProductsQueryService query)
  {
    _query = query;
  }

  public async ValueTask<Result<PagedResult<ProductDto>>> Handle(ListProductsQuery request,
                                                                 CancellationToken cancellationToken)
  {

    var result = await _query.ListAsync(request.Page ?? 1, request.PerPage ?? Constants.DEFAULT_PAGE_SIZE);

    return Result.Success(result);
  }
}
