using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimble.Modulith.Reporting.Models;

namespace Nimble.Modulith.Reporting.Data.Config;

public class DimProductConfig : IEntityTypeConfiguration<DimProduct>
{
    public void Configure(EntityTypeBuilder<DimProduct> builder)
    {
        builder.ToTable("DimProduct");
        
        builder.HasKey(p => p.ProductId);
        
        // Do NOT use identity - we control the IDs from source systems
        builder.Property(p => p.ProductId)
            .ValueGeneratedNever();
        
        builder.Property(p => p.ProductName)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(p => p.FirstSeenDate)
            .IsRequired();
        
        builder.Property(p => p.LastUpdatedDate)
            .IsRequired();
        
        builder.HasIndex(p => p.ProductName);
    }
}
