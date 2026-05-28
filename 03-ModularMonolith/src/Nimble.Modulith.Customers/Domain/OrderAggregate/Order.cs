using System.Diagnostics.CodeAnalysis;
using Nimble.Modulith.Customers.Domain.Common;

namespace Nimble.Modulith.Customers.Domain.OrderAggregate;

public class Order : EntityBase
{
    private readonly List<OrderItem> _items = new();

    public int CustomerId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateOnly OrderDate { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    public void AddItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        // Prevent modifications to confirmed orders
        if (Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException($"Cannot add items to an order with status '{Status}'. Only pending orders can be modified.");
        }

        // Check if an item with the same product already exists
        var existingItem = _items.FirstOrDefault(i => i.ProductId == item.ProductId);
        
        if (existingItem != null)
        {
            // Combine quantities for existing product
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            _items.Add(item);
        }
    }

    public void RemoveItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        
        // Prevent modifications to confirmed orders
        if (Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException($"Cannot remove items from an order with status '{Status}'. Only pending orders can be modified.");
        }
        
        _items.Remove(item);
    }
}
