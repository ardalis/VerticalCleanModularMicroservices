using OrderDemo.CleanArch.Core.ProductAggregate;

namespace OrderDemo.CleanArch.UseCases.Products.Get;

public record GetProductQuery(ProductId ProductId) : IQuery<Result<ProductDto>>;
