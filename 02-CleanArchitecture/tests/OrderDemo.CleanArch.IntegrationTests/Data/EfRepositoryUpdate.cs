using OrderDemo.CleanArch.Core.ProductAggregate;

namespace OrderDemo.CleanArch.IntegrationTests.Data;

public class EfRepositoryUpdate : BaseEfRepoTestFixture
{
  [Fact]
  public async Task UpdatesItemAfterAddingIt()
  {
    // add a Product
    var repository = GetRepository();
    var productId = ProductId.From(1);
    var initialUnitPrice = 0.55m;
    var initialName = Guid.NewGuid().ToString();
    var Product = new Product(productId, initialName, initialUnitPrice);

    await repository.AddAsync(Product);

    // detach the item so we get a different instance
    _dbContext.Entry(Product).State = EntityState.Detached;

    // fetch the item and update its title
    var newProduct = (await repository.ListAsync())
        .FirstOrDefault(Product => Product.Name == initialName);
    newProduct.ShouldNotBeNull();

    Product.ShouldNotBeSameAs(newProduct);
    var newName = Guid.NewGuid().ToString();
    newProduct.UpdateName(newName);

    // Update the item
    await repository.UpdateAsync(newProduct);

    // Fetch the updated item
    var updatedItem = (await repository.ListAsync())
        .FirstOrDefault(Product => Product.Name == newName);

    updatedItem.ShouldNotBeNull();
    Product.Name.ShouldNotBe(updatedItem.Name);
    newProduct.Id.ShouldBe(updatedItem.Id);
  }
}
