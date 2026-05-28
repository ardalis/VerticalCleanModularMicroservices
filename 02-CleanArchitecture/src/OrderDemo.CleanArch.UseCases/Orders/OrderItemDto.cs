namespace OrderDemo.CleanArch.UseCases.Orders;

public class OrderItemDto
  {
  public string Id { get; set; } = string.Empty;
  public int ProductId { get; set; }
  public string ProductName { get; set; } = string.Empty;
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
}
