using OrderDemo.CleanArch.Core.ProductAggregate;

namespace OrderDemo.CleanArch.UseCases.Products;
public record ProductDto(ProductId Id, string Name, decimal UnitPrice);
