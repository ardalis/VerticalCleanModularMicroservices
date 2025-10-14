namespace OrderDemo.Core.CartAggregate;

public class CartItem
{
  public Guid Id { get; set; }
  public Guid CartId { get; set; }
  public Cart? Cart { get; set; }
  public int ProductId { get; set; }
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
}
