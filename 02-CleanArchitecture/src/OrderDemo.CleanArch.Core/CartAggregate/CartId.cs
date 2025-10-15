using Vogen;

namespace OrderDemo.CleanArch.Core.CartAggregate;

[ValueObject<Guid>]
public readonly partial struct CartId
{
  private static Validation Validate(Guid value)
      => value != Guid.Empty ? Validation.Ok : Validation.Invalid("CartId cannot be empty.");
}
