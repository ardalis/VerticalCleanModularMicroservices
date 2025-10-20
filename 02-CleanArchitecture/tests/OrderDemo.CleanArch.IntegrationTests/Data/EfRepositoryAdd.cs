using OrderDemo.CleanArch.Core.ProductAggregate;

namespace OrderDemo.CleanArch.IntegrationTests.Data;

public class EfRepositoryAdd : BaseEfRepoTestFixture
{
  [Fact]
  public async Task AddsProductAndSetsId()
  {
    var testProductId = ProductId.From(1);
    var testProductName = "testProduct";
    decimal testProductUnitPrice = 10m;
    var repository = GetRepository();
    var Product = new Product(testProductId, testProductName, testProductUnitPrice);

    await repository.AddAsync(Product);

    var newProduct = (await repository.ListAsync())
                    .FirstOrDefault();

    newProduct.ShouldNotBeNull();
    testProductName.ShouldBe(newProduct.Name);
    testProductUnitPrice.ShouldBe(newProduct.UnitPrice);
    newProduct.Id.Value.ShouldBeGreaterThan(0);
  }
}
