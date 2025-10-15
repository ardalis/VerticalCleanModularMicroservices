namespace OrderDemo.CleanArch.Core.OrderAggregate;

public class OrderItem
{
  public OrderItem(int id, Guid orderId, int productId, int quantity, decimal unitPrice)
  {
    Id = id;
    OrderId = orderId;
    ProductId = productId;
    Quantity = quantity;
    UnitPrice = unitPrice;
  }

  public int Id { get; private set; }
  public Guid OrderId { get; private set; }
  public int ProductId { get; private set; }
  public int Quantity { get; private set; }
  public decimal UnitPrice { get; private set; }
}
