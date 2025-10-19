using FluentValidation;

namespace OrderDemo.CleanArch.Web.Cart;

public sealed class AddToCartValidator : Validator<AddToCartRequest>
{
  public AddToCartValidator()
  {
    RuleFor(x => x.ProductId)
      .GreaterThan(0)
      .WithMessage("Product ID must be greater than 0");

    RuleFor(x => x.Quantity)
      .GreaterThan(0)
      .WithMessage("Quantity must be greater than 0");
  }
}
