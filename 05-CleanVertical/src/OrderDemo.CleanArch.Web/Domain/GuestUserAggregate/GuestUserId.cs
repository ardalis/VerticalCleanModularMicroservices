﻿using Vogen;

namespace OrderDemo.CleanVertical.Web.Domain.GuestUserAggregate;

[ValueObject<Guid>]
public readonly partial struct GuestUserId
{
  private static Validation Validate(Guid value)
      => value != Guid.Empty ? Validation.Ok : Validation.Invalid("GuestUserId cannot be empty.");
}
