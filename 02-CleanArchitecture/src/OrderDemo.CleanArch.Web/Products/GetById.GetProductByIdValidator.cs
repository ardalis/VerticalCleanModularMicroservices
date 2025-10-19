using FluentValidation;

namespace OrderDemo.CleanArch.Web.Products;

public sealed class GetProductByIdValidator : Validator<GetProductByIdRequest>
{
  public GetProductByIdValidator()
  {
    RuleFor(x => x.ProductId)
      .GreaterThan(0)
      .WithMessage("Product ID must be greater than 0");
  }
}
