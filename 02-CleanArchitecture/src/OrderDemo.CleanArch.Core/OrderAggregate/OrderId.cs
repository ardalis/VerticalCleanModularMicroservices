using Vogen;

namespace OrderDemo.CleanArch.Core.OrderAggregate;

[ValueObject<Guid>]
public readonly partial struct OrderId
{
  private static Validation Validate(Guid value)
      => value != Guid.Empty ? Validation.Ok : Validation.Invalid("OrderId cannot be empty.");
}
