﻿using Vogen;

namespace OrderDemo.CleanVertical.Core.OrderAggregate;

[ValueObject<int>]
public readonly partial struct Quantity
{
  private static Validation Validate(int value)
      => value > 0 ? Validation.Ok : Validation.Invalid("Quantity must be greater than zero.");

  public static Quantity operator +(Quantity left, Quantity right)
  {
    return From(left.Value + right.Value);
  }
}
