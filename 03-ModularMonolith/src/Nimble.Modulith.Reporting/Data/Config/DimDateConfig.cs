using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nimble.Modulith.Reporting.Models;

namespace Nimble.Modulith.Reporting.Data.Config;

public class DimDateConfig : IEntityTypeConfiguration<DimDate>
{
    public void Configure(EntityTypeBuilder<DimDate> builder)
    {
        builder.ToTable("DimDate");
        
        builder.HasKey(d => d.DateKey);
        
        // Do NOT use identity - DateKey is YYYYMMDD format
        builder.Property(d => d.DateKey)
            .ValueGeneratedNever();
        
        builder.Property(d => d.Date)
            .IsRequired();
        
        builder.Property(d => d.DayName)
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(d => d.MonthName)
            .HasMaxLength(20)
            .IsRequired();
        
        builder.HasIndex(d => d.Date)
            .IsUnique();
        
        // Seed all dates for 2025
        var dates = GenerateDateDimension(2025);
        builder.HasData(dates);
    }
    
    private static List<DimDate> GenerateDateDimension(int year)
    {
        var dates = new List<DimDate>();
        var startDate = new DateTime(year, 1, 1);
        var endDate = new DateTime(year, 12, 31);
        
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            dates.Add(new DimDate
            {
                DateKey = int.Parse(date.ToString("yyyyMMdd")),
                Date = date,
                Year = date.Year,
                Quarter = (date.Month - 1) / 3 + 1,
                Month = date.Month,
                Day = date.Day,
                DayOfWeek = (int)date.DayOfWeek,
                DayName = date.DayOfWeek.ToString(),
                MonthName = date.ToString("MMMM")
            });
        }
        
        return dates;
    }
}
