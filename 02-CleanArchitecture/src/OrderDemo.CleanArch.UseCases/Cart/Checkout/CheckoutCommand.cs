using OrderDemo.CleanArch.Core.CartAggregate;
using OrderDemo.CleanArch.Core.GuestUserAggregate;
using OrderDemo.CleanArch.Core.OrderAggregate;

namespace OrderDemo.CleanArch.UseCases.Cart.Checkout;

public record CheckoutCommand(CartId CartId, GuestUserId GuestUserId) : ICommand<Result<CheckoutResult>>;

public record CheckoutResult(string Message, OrderId OrderId);