using Microsoft.EntityFrameworkCore;
using Nimble.Modulith.Reporting.Models;

namespace Nimble.Modulith.Reporting.Data;

public class ReportingDbContext : DbContext
{
    public ReportingDbContext(DbContextOptions<ReportingDbContext> options) : base(options)
    {
    }
    
    public DbSet<DimDate> DimDates { get; set; }
    public DbSet<DimCustomer> DimCustomers { get; set; }
    public DbSet<DimProduct> DimProducts { get; set; }
    public DbSet<FactOrder> FactOrders { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReportingDbContext).Assembly);
    }
}
