using OrderDemo.CleanVertical.Web.Domain.ProductAggregate;

namespace OrderDemo.CleanArch.UseCases.Products;
public record ProductDto(ProductId Id, string Name, decimal UnitPrice);
