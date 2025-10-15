namespace OrderDemo.CleanArch.Core.CartAggregate;

public class Cart : EntityBase<Cart, CartId>, IAggregateRoot
{
  private readonly List<CartItem> _items = new();

  public Cart(CartId id)
  {
    Id = id;
    CreatedOn = DateTime.UtcNow;
  }

  public DateTime CreatedOn { get; private set; }
  public bool Deleted { get; private set; } = false;
  public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

  public void AddItem(int productId, int quantity, decimal unitPrice)
  {
    var item = new CartItem(Guid.NewGuid(), Id.Value, productId, quantity, unitPrice);
    _items.Add(item);
  }

  public void MarkAsDeleted()
  {
    Deleted = true;
  }
}
