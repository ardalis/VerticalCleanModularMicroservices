using OrderDemo.CleanVertical.Web.ProductFeatures.List;

namespace OrderDemo.CleanVertical.FunctionalTests.ApiEndpoints;

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
