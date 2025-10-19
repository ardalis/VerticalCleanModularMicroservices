namespace OrderDemo.CleanArch.Core.CartAggregate;

public class CartItem : EntityBase<CartItem, Guid>
{
  // Private constructor for EF Core
  private CartItem() { }
  
  public CartItem(Guid id, int productId, int quantity, decimal unitPrice)
  {
    Id = id;
    ProductId = productId;
    Quantity = quantity;
    UnitPrice = unitPrice;
  }

  public int ProductId { get; private set; }
  public int Quantity { get; private set; }
  public decimal UnitPrice { get; private set; }
}
