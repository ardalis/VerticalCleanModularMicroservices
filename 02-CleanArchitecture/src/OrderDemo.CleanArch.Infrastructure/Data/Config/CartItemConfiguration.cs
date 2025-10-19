using OrderDemo.CleanArch.Core.CartAggregate;

namespace OrderDemo.CleanArch.Infrastructure.Data.Config;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
  public void Configure(EntityTypeBuilder<CartItem> builder)
  {
    builder.ToTable("CartItems");
    
    builder.HasKey(ci => ci.Id);
    
    // Use SQL Server IDENTITY for auto-increment int ID
    builder.Property(ci => ci.Id)
      .ValueGeneratedOnAdd()
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
