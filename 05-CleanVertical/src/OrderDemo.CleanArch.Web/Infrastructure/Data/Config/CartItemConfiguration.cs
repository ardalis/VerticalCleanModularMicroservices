﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderDemo.CleanVertical.Web.Domain.CartAggregate;

namespace OrderDemo.CleanVertical.Web.Infrastructure.Data.Config;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
  public void Configure(EntityTypeBuilder<CartItem> builder)
  {
    builder.ToTable("CartItems");

    builder.Property(entity => entity.Id)
      .HasValueGenerator<VogenGuidIdValueGenerator<AppDbContext, CartItem, CartItemId>>()
      .HasVogenConversion()
      .IsRequired();
        
    builder.Property(ci => ci.ProductId)
      .IsRequired();
    
    builder.Property(ci => ci.Quantity)
      .IsRequired();
    
    builder.Property(ci => ci.UnitPrice)
      .HasPrecision(18, 2)
      .IsRequired();
  }
}
