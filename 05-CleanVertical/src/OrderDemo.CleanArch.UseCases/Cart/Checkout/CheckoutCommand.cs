using OrderDemo.CleanVertical.Web.Domain.CartAggregate;
using OrderDemo.CleanVertical.Web.Domain.OrderAggregate;

namespace OrderDemo.CleanVertical.UseCases.Cart.Checkout;

public record CheckoutCommand(CartId CartId, string Email) : ICommand<Result<CheckoutResult>>;

public record CheckoutResult(OrderId OrderId);
