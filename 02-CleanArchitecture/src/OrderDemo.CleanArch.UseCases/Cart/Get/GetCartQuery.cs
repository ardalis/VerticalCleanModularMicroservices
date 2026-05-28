using OrderDemo.CleanArch.Core.CartAggregate;

namespace OrderDemo.CleanArch.UseCases.Cart.Get;

public record GetCartQuery(CartId CartId) : IQuery<Result<CartDto>>;