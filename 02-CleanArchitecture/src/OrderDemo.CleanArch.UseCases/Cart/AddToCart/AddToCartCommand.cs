using OrderDemo.CleanArch.Core.CartAggregate;

namespace OrderDemo.CleanArch.UseCases.Cart.AddToCart;

public record AddToCartCommand(CartId? CartId, int ProductId, int Quantity) : ICommand<Result<CartDto>>;
