using FluentValidation;

namespace OrderDemo.CleanArch.Web.Cart;

public sealed class CheckoutValidator : Validator<CheckoutRequest>
{
  public CheckoutValidator()
  {
    RuleFor(x => x.CartId)
      .NotEmpty()
      .WithMessage("Cart ID is required");

    RuleFor(x => x.Email)
      .NotEmpty()
      .WithMessage("Email is required")
      .EmailAddress()
      .WithMessage("Email must be a valid email address");
  }
}