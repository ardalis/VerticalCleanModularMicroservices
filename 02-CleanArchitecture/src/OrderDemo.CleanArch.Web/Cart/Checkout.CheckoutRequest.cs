namespace OrderDemo.CleanArch.Web.Cart;

public sealed class CheckoutRequest
{
  public const string Route = "/cart/{CartId}/checkout";
  
  public Guid CartId { get; init; }
  public string Email { get; init; } = string.Empty;
}