namespace OrderDemo.CleanArch.Web.Cart;

public sealed class GetCartRequest
{
  public const string Route = "/cart/{CartId}";
  
  public Guid CartId { get; init; }
}