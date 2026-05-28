using OrderDemo.CleanVertical.Web.Domain.CartAggregate;

namespace OrderDemo.CleanVertical.UseCases.Cart;

public record CartDto(CartId Id, IReadOnlyList<CartItemDto> Items, decimal Total);

public record CartItemDto(int ProductId, int Quantity, decimal UnitPrice, decimal TotalPrice);
