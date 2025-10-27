using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimble.Modulith.Reporting.Models;

namespace Nimble.Modulith.Reporting.Data.Config;

public class DimCustomerConfig : IEntityTypeConfiguration<DimCustomer>
{
    public void Configure(EntityTypeBuilder<DimCustomer> builder)
    {
        builder.ToTable("DimCustomer");
        
        builder.HasKey(c => c.CustomerId);
        
        // Do NOT use identity - we control the IDs from source systems
        builder.Property(c => c.CustomerId)
            .ValueGeneratedNever();
        
        builder.Property(c => c.CustomerEmail)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(c => c.CustomerName)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(c => c.FirstSeenDate)
            .IsRequired();
        
        builder.Property(c => c.LastUpdatedDate)
            .IsRequired();
        
        builder.HasIndex(c => c.CustomerEmail);
    }
}
