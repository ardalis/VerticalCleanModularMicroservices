﻿
using OrderDemo.CleanArch.Core.ProductAggregate;

namespace OrderDemo.CleanArch.Core.OrderAggregate;

public class OrderItem(OrderId orderId, ProductId productId, Quantity quantity, Price unitPrice)
  : EntityBase<OrderItem, OrderItemId>
{
  public OrderId OrderId { get; private set; } = orderId;
  public ProductId ProductId { get; private set; } = productId;
  public Quantity Quantity { get; private set; } = quantity;
  public Price UnitPrice { get; private set; } = unitPrice;

  internal void IncreaseQuantity(Quantity quantity)
  {
    Quantity += quantity;
  }
}
