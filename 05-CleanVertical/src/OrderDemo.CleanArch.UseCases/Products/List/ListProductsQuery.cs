using OrderDemo.CleanArch.UseCases.Products;
using OrderDemo.CleanArch.Web.ProductFeatures;

namespace OrderDemo.CleanVertical.UseCases.Products.List;

public record ListProductsQuery(int? Page = 1, int? PerPage = Constants.DEFAULT_PAGE_SIZE)
  : IQuery<Result<PagedResult<ProductDto>>>;
