namespace OrderDemo.Core.OrderAggregate;

public class OrderItem
{
  public int Id { get; set; }

  public Guid OrderId { get; set; }
  public Order? Order { get; set; }

  public int ProductId { get; set; }

  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
}
