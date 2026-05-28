using OrderDemo.CleanArch.Core.CartAggregate;
using OrderDemo.CleanArch.Core.OrderAggregate;

namespace OrderDemo.CleanArch.UseCases.Cart.Checkout;

public record CheckoutCommand(CartId CartId, string Email) : ICommand<Result<CheckoutResult>>;

public record CheckoutResult(OrderId OrderId);
