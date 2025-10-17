using OrderDemo.CleanArch.Infrastructure.Data;
using OrderDemo.CleanArch.Web.Products;

namespace OrderDemo.CleanArch.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class ProductList(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client = factory.CreateClient();

  [Fact]
  public async Task ReturnsTenProducts()
  {
    var result = await _client.GetAndDeserializeAsync<ProductListResponse>("/Products");

    Assert.Equal(SeedData.NUMBER_OF_PRODUCTS, result.TotalCount);
    Assert.Contains(result.Items, i => i.Name == SeedData.Product1.Name);
    Assert.Contains(result.Items, i => i.Name == SeedData.Product2.Name);
  }
}
