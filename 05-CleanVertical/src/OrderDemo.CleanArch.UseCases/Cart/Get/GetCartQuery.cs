using OrderDemo.CleanVertical.Web.Domain.CartAggregate;

namespace OrderDemo.CleanVertical.UseCases.Cart.Get;

public record GetCartQuery(CartId CartId) : IQuery<Result<CartDto>>;