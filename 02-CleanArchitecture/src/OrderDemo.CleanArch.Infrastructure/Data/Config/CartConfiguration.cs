using OrderDemo.CleanArch.Core.CartAggregate;

namespace OrderDemo.CleanArch.Infrastructure.Data.Config;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
  public void Configure(EntityTypeBuilder<Cart> builder)
  {
    // Store Id as Guid in database, convert to/from CartId in application
    builder.Property(entity => entity.Id)
      .HasValueGenerator<VogenIdValueGenerator<AppDbContext, Cart, CartId>>()
      .HasVogenConversion()
      .IsRequired();

    builder.Property(entity => entity.CreatedOn)
      .IsRequired();

    builder.Property(entity => entity.Deleted)
      .IsRequired();

    // Simple relationship - EF will handle the foreign key
    builder.HasMany(c => c.Items);
  }
}
