using Microsoft.EntityFrameworkCore;
using Nimble.Modulith.Customers.Domain.CustomerAggregate;
using Nimble.Modulith.Customers.Domain.OrderAggregate;

namespace Nimble.Modulith.Customers.Infrastructure.Data;

public class CustomersDbContext : DbContext
{
    public CustomersDbContext(DbContextOptions<CustomersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Apply Customers module specific configurations
        builder.HasDefaultSchema("Customers");
        
        // Apply all configurations from this assembly
        builder.ApplyConfigurationsFromAssembly(typeof(CustomersDbContext).Assembly);
    }
}
