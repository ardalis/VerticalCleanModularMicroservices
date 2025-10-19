using OrderDemo.CleanArch.Core.CartAggregate;
using OrderDemo.CleanArch.Core.ContributorAggregate;
using OrderDemo.CleanArch.Core.ProductAggregate;

namespace OrderDemo.CleanArch.Infrastructure.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Contributor> Contributors => Set<Contributor>();
  public DbSet<Product> Products => Set<Product>();
  public DbSet<Cart> Carts => Set<Cart>();
  public DbSet<CartItem> CartItems => Set<CartItem>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override int SaveChanges() =>
        SaveChangesAsync().GetAwaiter().GetResult();
}
