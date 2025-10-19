using Vogen;

namespace OrderDemo.CleanArch.Core.CartAggregate;

[ValueObject<int>]
public readonly partial struct CartId
{
  private static Validation Validate(int value)
      => value > 0 ? Validation.Ok : Validation.Invalid("CartId must be positive.");
}
