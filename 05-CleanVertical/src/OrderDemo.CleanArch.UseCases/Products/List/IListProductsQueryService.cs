using OrderDemo.CleanArch.UseCases.Products;
using OrderDemo.CleanArch.Web.ProductFeatures;

namespace OrderDemo.CleanVertical.UseCases.Products.List;

/// <summary>
/// Represents a service that will actually fetch the necessary data
/// Typically implemented in Infrastructure
/// </summary>
public interface IListProductsQueryService
{
  Task<UseCases.PagedResult<ProductDto>> ListAsync(int page, int perPage);
}
