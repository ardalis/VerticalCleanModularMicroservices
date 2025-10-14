namespace OrderDemo.Api.Data.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    public List<OrderItem> Items { get; private set; } = new();

    public decimal Total => Items.Sum(i => i.UnitPrice * i.Quantity);
}
