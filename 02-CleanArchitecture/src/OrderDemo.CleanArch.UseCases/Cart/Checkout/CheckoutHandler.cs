using OrderDemo.CleanArch.Core.CartAggregate;
using OrderDemo.CleanArch.Core.CartAggregate.Specifications;
using OrderDemo.CleanArch.Core.GuestUserAggregate;
using OrderDemo.CleanArch.Core.GuestUserAggregate.Specifications;
using OrderDemo.CleanArch.Core.OrderAggregate;
using OrderDemo.CleanArch.Core.ProductAggregate;

namespace OrderDemo.CleanArch.UseCases.Cart.Checkout;

public class CheckoutHandler(
  IRepository<Core.CartAggregate.Cart> cartRepository,
  IRepository<GuestUser> guestUserRepository,
  IRepository<Order> orderRepository)
  : ICommandHandler<CheckoutCommand, Result<CheckoutResult>>
{
  public async ValueTask<Result<CheckoutResult>> Handle(CheckoutCommand request, CancellationToken cancellationToken)
  {
    // Find the cart with its items
    var cartSpec = new CartByIdSpec(request.CartId);
    var cart = await cartRepository.FirstOrDefaultAsync(cartSpec, cancellationToken);
    
    if (cart == null)
    {
      return Result.NotFound("Cart not found");
    }

    if (!cart.Items.Any())
    {
      return Result.Invalid("Cart is empty");
    }

    // Get or create guest user
    var guestUserSpec = new GuestUserByIdSpec(request.GuestUserId);
    var guestUser = await guestUserRepository.FirstOrDefaultAsync(guestUserSpec, cancellationToken);
    
    if (guestUser == null)
    {
      guestUser = new GuestUser(request.GuestUserId, "guest@example.com");
      guestUser = await guestUserRepository.AddAsync(guestUser, cancellationToken);
    }

    // Create new order
    var orderId = OrderId.From(Guid.NewGuid());
    var order = new Order(orderId, guestUser.Id.Value);

    // Add items from cart to order
    foreach (var cartItem in cart.Items)
    {
      order.AddItem(
        ProductId.From(cartItem.ProductId),
        Quantity.From(cartItem.Quantity),
        Price.From(cartItem.UnitPrice));
    }

    // Save the order
    await orderRepository.AddAsync(order, cancellationToken);

    // Mark cart as deleted
    cart.MarkAsDeleted();
    await cartRepository.UpdateAsync(cart, cancellationToken);

    return new CheckoutResult("Checkout successful", order.Id);
  }
}