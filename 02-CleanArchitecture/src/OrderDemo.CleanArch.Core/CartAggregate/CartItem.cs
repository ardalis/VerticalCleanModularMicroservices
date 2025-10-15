namespace OrderDemo.CleanArch.Core.CartAggregate;

public class CartItem
{
  public CartItem(Guid id, Guid cartId, int productId, int quantity, decimal unitPrice)
  {
    Id = id;
    CartId = cartId;
    ProductId = productId;
    Quantity = quantity;
    UnitPrice = unitPrice;
  }

  public Guid Id { get; private set; }
  public Guid CartId { get; private set; }
  public int ProductId { get; private set; }
  public int Quantity { get; private set; }
  public decimal UnitPrice { get; private set; }
}
