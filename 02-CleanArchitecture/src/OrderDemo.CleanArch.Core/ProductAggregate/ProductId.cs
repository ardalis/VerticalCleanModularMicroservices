using Vogen;

namespace OrderDemo.CleanArch.Core.ProductAggregate;

[ValueObject<int>]
public readonly partial struct ProductId
{
  private static Validation Validate(int value)
      => value > 0 ? Validation.Ok : Validation.Invalid("ProductId must be positive.");
}
