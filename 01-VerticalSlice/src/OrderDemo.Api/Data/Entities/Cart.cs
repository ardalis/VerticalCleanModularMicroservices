namespace OrderDemo.Api.Data.Entities;

public class Cart
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public bool Deleted { get; set; } = false;
    public List<CartItem> Items { get; set; } = new();
}

public class CartItem
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Cart? Cart { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}