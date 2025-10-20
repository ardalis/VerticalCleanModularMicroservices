using OrderDemo.CleanArch.Infrastructure.Data;
using OrderDemo.CleanArch.Web.Products;

namespace OrderDemo.CleanArch.FunctionalTests.ApiEndpoints;

[Collection("Sequential")]
public class ProductList(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client = factory.CreateClient();

  [Fact]
  public async Task ReturnsProducts()
  {
    var result = await _client.GetAndDeserializeAsync<ProductListResponse>("/Products");

    Assert.Equal(3, result.TotalCount); // currently hits the real db
  }
}
