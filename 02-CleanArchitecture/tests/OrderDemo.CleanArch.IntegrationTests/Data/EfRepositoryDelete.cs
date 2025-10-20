using OrderDemo.CleanArch.Core.ProductAggregate;

namespace OrderDemo.CleanArch.IntegrationTests.Data;

public class EfRepositoryDelete : BaseEfRepoTestFixture
{
  [Fact]
  public async Task DeletesItemAfterAddingIt()
  {
    // add a Product
    var repository = GetRepository();
    var initialName = Guid.NewGuid().ToString();
    var Product = new Product(ProductId.From(1), initialName, 0.55m);
    await repository.AddAsync(Product);

    // delete the item
    await repository.DeleteAsync(Product);

    // verify it's no longer there
    (await repository.ListAsync()).ShouldNotContain(Product => Product.Name == initialName);
  }
}
