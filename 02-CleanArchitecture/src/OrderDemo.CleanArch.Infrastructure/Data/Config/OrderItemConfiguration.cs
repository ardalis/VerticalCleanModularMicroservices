using OrderDemo.CleanArch.Core.OrderAggregate;
using OrderDemo.CleanArch.Core.ProductAggregate;

namespace OrderDemo.CleanArch.Infrastructure.Data.Config;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
  public void Configure(EntityTypeBuilder<OrderItem> builder)
  {
    builder.ToTable("OrderItems");

    builder.Property(entity => entity.Id)
      .HasValueGenerator<VogenGuidIdValueGenerator<AppDbContext, OrderItem, OrderItemId>>()
      .HasVogenConversion()
      .IsRequired();
        
    builder.Property(oi => oi.OrderId)
      .HasVogenConversion()
      .IsRequired();
    
    builder.Property(oi => oi.ProductId)
      .HasVogenConversion()
      .IsRequired();
    
    builder.Property(oi => oi.Quantity)
      .HasVogenConversion()
      .IsRequired();
    
    builder.Property(oi => oi.UnitPrice)
      .HasVogenConversion()
      .HasPrecision(18, 2)
      .IsRequired();
  }
}