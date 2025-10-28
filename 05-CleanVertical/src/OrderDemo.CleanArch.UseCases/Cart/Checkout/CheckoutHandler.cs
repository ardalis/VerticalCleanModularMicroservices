﻿using OrderDemo.CleanVertical.Web.Domain.CartAggregate;
using OrderDemo.CleanVertical.Web.Domain.CartAggregate.Specifications;
using OrderDemo.CleanVertical.Web.Domain.GuestUserAggregate;
using OrderDemo.CleanVertical.Web.Domain.GuestUserAggregate.Specifications;
using OrderDemo.CleanVertical.Web.Domain.OrderAggregate;
using OrderDemo.CleanVertical.Web.Domain.ProductAggregate;

namespace OrderDemo.CleanVertical.UseCases.Cart.Checkout;

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
    
    if (cart == null) return Result.NotFound("Cart not found");
    if (!cart.Items.Any()) return Result.Invalid(new ValidationError("Cart is empty"));

    // Get or create guest user
    var guestUserSpec = new GuestUserByEmailSpec(request.Email);
    var guestUser = await guestUserRepository.FirstOrDefaultAsync(guestUserSpec, cancellationToken);
    
    if (guestUser == null)
    {
      var guestUserId = GuestUserId.From(Guid.NewGuid());
      guestUser = new GuestUser(guestUserId, request.Email);
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

    await orderRepository.AddAsync(order, cancellationToken);

    cart.MarkAsDeleted();
    await cartRepository.UpdateAsync(cart, cancellationToken);

    return new CheckoutResult(order.Id);
  }
}
