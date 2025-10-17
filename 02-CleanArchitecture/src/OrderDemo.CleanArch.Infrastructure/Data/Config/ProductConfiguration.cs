using OrderDemo.CleanArch.Core.ProductAggregate;

namespace OrderDemo.CleanArch.Infrastructure.Data.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> builder)
  {
    builder.Property(entity => entity.Id)
      .HasValueGenerator<VogenIdValueGenerator<AppDbContext, Product, ProductId>>()
      .HasVogenConversion()
      .IsRequired();

    builder.Property(entity => entity.Name)
      .HasMaxLength(100)
      .IsRequired();

    builder.Property(entity => entity.UnitPrice)
      .HasPrecision(18, 2)
      .IsRequired();
  }
}
