namespace OrderDemo.CleanArch.Web.Cart;

public sealed class AddToCartRequest
{
  public const string Route = "/cart";
  
  public int? CartId { get; init; }
  public int ProductId { get; init; }
  public int Quantity { get; init; }
}
