namespace OrderDemo.CleanArch.UseCases.Products.List;

public record ListProductsQuery(int? Page = 1, int? PerPage = Constants.DEFAULT_PAGE_SIZE)
  : IQuery<Result<PagedResult<ProductDto>>>;
