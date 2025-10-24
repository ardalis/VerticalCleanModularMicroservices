using Vogen;

namespace OrderDemo.CleanArch.Core.OrderAggregate;

[ValueObject<Guid>]
public readonly partial struct OrderItemId
{
  private static Validation Validate(Guid value)
      => value != Guid.Empty ? Validation.Ok : Validation.Invalid("OrderItemId cannot be empty.");
}

[ValueObject<int>]
public readonly partial struct Quantity
{
  private static Validation Validate(int value)
      => value > 0 ? Validation.Ok : Validation.Invalid("Quantity must be greater than zero.");
}

[ValueObject<decimal>]
public readonly partial struct Price
{
  private static Validation Validate(decimal value)
      => value > 0 ? Validation.Ok : Validation.Invalid("Price must be greater than zero.");
}
