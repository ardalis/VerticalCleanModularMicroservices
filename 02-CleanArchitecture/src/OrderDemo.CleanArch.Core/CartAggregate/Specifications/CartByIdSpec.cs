namespace OrderDemo.CleanArch.Core.CartAggregate.Specifications;

public class CartByIdSpec : Specification<Cart>
{
  public CartByIdSpec(CartId cartId) =>
    Query
        .Where(cart => cart.Id == cartId && !cart.Deleted);
}
