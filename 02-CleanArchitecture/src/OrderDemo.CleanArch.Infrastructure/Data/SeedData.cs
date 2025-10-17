using OrderDemo.CleanArch.Core.ContributorAggregate;
using OrderDemo.CleanArch.Core.ProductAggregate;

namespace OrderDemo.CleanArch.Infrastructure.Data;

public static class SeedData
{
  public const int NUMBER_OF_CONTRIBUTORS = 27; // including the 2 below
  public static readonly Contributor Contributor1 = new(ContributorName.From("Ardalis"));
  public static readonly Contributor Contributor2 = new(ContributorName.From("Ilyana"));

  public const int NUMBER_OF_PRODUCTS = 10;
  public static readonly Product Product1 = new(ProductId.From(0), "Widget", 9.99m);
  public static readonly Product Product2 = new(ProductId.From(0), "Gadget", 19.99m);

  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    if (await dbContext.Contributors.AnyAsync() || await dbContext.Products.AnyAsync()) return; // DB has been seeded

    await PopulateTestDataAsync(dbContext);
  }

  public static async Task PopulateTestDataAsync(AppDbContext dbContext)
  {
    dbContext.Contributors.AddRange([Contributor1, Contributor2]);
    await dbContext.SaveChangesAsync();

    // add a bunch more contributors to support demonstrating paging
    for (int i = 1; i <= NUMBER_OF_CONTRIBUTORS-2; i++)
    {
      dbContext.Contributors.Add(new Contributor(ContributorName.From($"Contributor {i}")));
    }
    await dbContext.SaveChangesAsync();

    // seed products
    dbContext.Products.AddRange([Product1, Product2]);
    await dbContext.SaveChangesAsync();

    // add more products to support demonstrating paging
    for (int i = 1; i <= NUMBER_OF_PRODUCTS-2; i++)
    {
      dbContext.Products.Add(new Product(ProductId.From(0), $"Product {i}", 10m + i));
    }
    await dbContext.SaveChangesAsync();
  }
}
