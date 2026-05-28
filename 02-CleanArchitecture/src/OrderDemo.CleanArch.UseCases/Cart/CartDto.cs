using OrderDemo.CleanArch.Core.CartAggregate;

namespace OrderDemo.CleanArch.UseCases.Cart;

public record CartDto(CartId Id, IReadOnlyList<CartItemDto> Items, decimal Total);

public record CartItemDto(int ProductId, int Quantity, decimal UnitPrice, decimal TotalPrice);
