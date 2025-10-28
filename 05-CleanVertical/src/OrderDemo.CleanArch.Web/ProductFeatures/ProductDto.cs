using OrderDemo.CleanVertical.Web.Domain.ProductAggregate;

namespace OrderDemo.CleanVertical.Web.ProductFeatures;
public record ProductDto(ProductId Id, string Name, decimal UnitPrice);
