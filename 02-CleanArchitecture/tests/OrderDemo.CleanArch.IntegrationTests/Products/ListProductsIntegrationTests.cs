using OrderDemo.CleanArch.Core.ProductAggregate;
using OrderDemo.CleanArch.Infrastructure.Data;
using OrderDemo.CleanArch.Infrastructure.Data.Queries;

namespace OrderDemo.CleanArch.IntegrationTests.Products;

public class ListProductsIntegrationTests
{
  private AppDbContext _dbContext;
  private ListProductsQueryService _queryService;

  public ListProductsIntegrationTests()
  {
    var options = CreateNewContextOptions();
    _dbContext = new AppDbContext(options);
    _queryService = new ListProductsQueryService(_dbContext);
    
    // Seed test data
    SeedTestProducts().Wait();
  }

  private static DbContextOptions<AppDbContext> CreateNewContextOptions()
  {
    var fakeEventDispatcher = Substitute.For<IDomainEventDispatcher>();
    var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .AddScoped<IDomainEventDispatcher>(_ => fakeEventDispatcher)
        .AddScoped<EventDispatchInterceptor>()
        .BuildServiceProvider();

    var interceptor = serviceProvider.GetRequiredService<EventDispatchInterceptor>();

    var builder = new DbContextOptionsBuilder<AppDbContext>();
    builder.UseInMemoryDatabase("listproductstest")
           .UseInternalServiceProvider(serviceProvider)
           .AddInterceptors(interceptor);

    return builder.Options;
  }

  private async Task SeedTestProducts()
  {
    // Add test products
    for (int i = 1; i <= 10; i++)
    {
      var product = new Product(ProductId.From(0), $"Product {i}", 10m + i);
      _dbContext.Products.Add(product);
    }
    await _dbContext.SaveChangesAsync();
  }

  [Theory]
  [InlineData(1, 5, 5, 10, 2)]  // page 1, pageSize 5, expectedItems 5, totalCount 10, totalPages 2
  [InlineData(2, 5, 5, 10, 2)]  // page 2, pageSize 5, expectedItems 5, totalCount 10, totalPages 2
  [InlineData(1, 10, 10, 10, 1)] // page 1, pageSize 10, expectedItems 10, totalCount 10, totalPages 1
  [InlineData(1, 3, 3, 10, 4)]  // page 1, pageSize 3, expectedItems 3, totalCount 10, totalPages 4
  public async Task ListProducts_ReturnsPagedListOfProducts(
    int page, 
    int pageSize, 
    int expectedItems, 
    int expectedTotalCount, 
    int expectedTotalPages)
  {
    // Act
    var result = await _queryService.ListAsync(page, pageSize);

    // Assert
    result.ShouldNotBeNull();
    result.Items.Count.ShouldBe(expectedItems);
    result.TotalCount.ShouldBe(expectedTotalCount);
    result.Page.ShouldBe(page);
    result.PerPage.ShouldBe(pageSize);
    result.TotalPages.ShouldBe(expectedTotalPages);
    
    // Verify each product has required properties
    foreach (var product in result.Items)
    {
      product.Id.ShouldNotBeNull();
      product.Name.ShouldNotBeNullOrWhiteSpace();
      product.UnitPrice.ShouldBeGreaterThan(0);
    }
  }

  [Fact]
  public async Task ListProducts_FirstPage_ReturnsProductsInOrder()
  {
    // Act
    var result = await _queryService.ListAsync(1, 10);

    // Assert
    result.Items.Count.ShouldBe(10);
    result.Items.First().Name.ShouldBe("Product 1");
    result.Items.Last().Name.ShouldBe("Product 10");
  }

  [Fact]
  public async Task ListProducts_EmptyDatabase_ReturnsEmptyList()
  {
    // Arrange - create a new context with no products
    var options = CreateNewContextOptions();
    var emptyDbContext = new AppDbContext(options);
    var emptyQueryService = new ListProductsQueryService(emptyDbContext);

    // Act
    var result = await emptyQueryService.ListAsync(1, 10);

    // Assert
    result.Items.Count.ShouldBe(0);
    result.TotalCount.ShouldBe(0);
    result.TotalPages.ShouldBe(0);
  }
}
