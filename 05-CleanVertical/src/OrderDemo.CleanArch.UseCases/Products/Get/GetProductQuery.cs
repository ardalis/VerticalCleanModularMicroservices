using OrderDemo.CleanArch.UseCases.Products;
using OrderDemo.CleanArch.Web.ProductFeatures;
using OrderDemo.CleanVertical.Web.Domain.ProductAggregate;

namespace OrderDemo.CleanVertical.UseCases.Products.Get;

public record GetProductQuery(ProductId ProductId) : IQuery<Result<ProductDto>>;
