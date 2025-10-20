
namespace OrderDemo.CleanArch.Core.OrderAggregate;

public class OrderItem(OrderId orderId, int productId, int quantity, decimal unitPrice)
  : EntityBase<OrderItem, OrderItemId>
{
  public OrderId OrderId { get; private set; } = orderId;
  public int ProductId { get; private set; } = productId;
  public int Quantity { get; private set; } = quantity;
  public decimal UnitPrice { get; private set; } = unitPrice;

  internal void IncreaseQuantity(int quantity)
  {
    Guard.Against.NegativeOrZero(quantity, nameof(quantity));
    Quantity += quantity;
  }
}
