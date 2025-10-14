namespace OrderDemo.Core.CartAggregate;

public class Cart
{
  public Guid Id { get; set; }
  public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
  public bool Deleted { get; set; } = false;
  public List<CartItem> Items { get; set; } = new();
}
