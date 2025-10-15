namespace OrderDemo.CleanArch.Core.OrderAggregate;

public class Order : EntityBase<Order, OrderId>, IAggregateRoot
{
  private readonly List<OrderItem> _items = new();

  public Order(OrderId id, Guid guestUserId)
  {
    Id = id;
    GuestUserId = guestUserId;
    CreatedOn = DateTimeOffset.UtcNow;
  }

  public DateTimeOffset CreatedOn { get; private set; }
  public Guid GuestUserId { get; private set; }
  public DateTimeOffset? DatePaid { get; private set; }
  public string PaymentReference { get; private set; } = string.Empty;
  public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

  public decimal Total => _items.Sum(i => i.UnitPrice * i.Quantity);

  public void AddItem(int productId, int quantity, decimal unitPrice)
  {
    var item = new OrderItem(0, Id.Value, productId, quantity, unitPrice);
    _items.Add(item);
  }

  public void ConfirmPayment(DateTimeOffset datePaid, string paymentReference)
  {
    DatePaid = datePaid;
    PaymentReference = paymentReference;
  }
}
