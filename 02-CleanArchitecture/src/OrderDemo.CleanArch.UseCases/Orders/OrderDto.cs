namespace OrderDemo.CleanArch.UseCases.Orders;

public class OrderDto
{
  public string Id { get; set; } = string.Empty;
  public DateTime OrderDate { get; set; }
  public decimal TotalAmount { get; set; }
}
