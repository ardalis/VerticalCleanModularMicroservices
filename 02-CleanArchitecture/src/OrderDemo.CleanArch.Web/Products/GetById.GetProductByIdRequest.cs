namespace OrderDemo.CleanArch.Web.Products;

public sealed class GetProductByIdRequest
{
  public const string Route = "/Products/{ProductId}";
  public int ProductId { get; init; }
}
