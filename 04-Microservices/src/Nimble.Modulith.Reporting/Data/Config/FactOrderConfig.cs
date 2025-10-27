using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimble.Modulith.Reporting.Models;

namespace Nimble.Modulith.Reporting.Data.Config;

public class FactOrderConfig : IEntityTypeConfiguration<FactOrder>
{
    public void Configure(EntityTypeBuilder<FactOrder> builder)
    {
        builder.ToTable("FactOrders");
        
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.OrderNumber)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(f => f.UnitPrice)
            .HasPrecision(18, 2);
        
        builder.Property(f => f.TotalPrice)
            .HasPrecision(18, 2);
        
        builder.Property(f => f.OrderTotalAmount)
            .HasPrecision(18, 2);
        
        builder.Property(f => f.IngestedAt)
            .IsRequired();
        
        // Relationships to dimension tables
        builder.HasOne(f => f.Date)
            .WithMany(d => d.Orders)
            .HasForeignKey(f => f.DateKey)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(f => f.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(f => f.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(f => f.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(f => f.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Indexes for common queries
        builder.HasIndex(f => f.DateKey);
        builder.HasIndex(f => f.CustomerId);
        builder.HasIndex(f => f.ProductId);
        builder.HasIndex(f => f.OrderId);
        builder.HasIndex(f => new { f.OrderId, f.OrderItemId })
            .IsUnique(); // Prevent duplicate ingestion
    }
}
