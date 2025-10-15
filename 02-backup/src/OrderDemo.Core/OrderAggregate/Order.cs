namespace OrderDemo.Core.OrderAggregate;

public class Order
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public DateTimeOffset CreatedOn { get; private set; } = DateTimeOffset.UtcNow;
  public List<OrderItem> Items { get; private set; } = new();

  public decimal Total => Items.Sum(i => i.UnitPrice * i.Quantity);

  public Guid? GuestUserId { get; set; }

  public DateTimeOffset? DatePaid { get; set; }
  public string PaymentReference { get; set; } = string.Empty;
}
