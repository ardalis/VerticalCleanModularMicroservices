using OrderDemo.CleanArch.Core.CartAggregate;
using OrderDemo.CleanArch.Core.CartAggregate.Specifications;
using OrderDemo.CleanArch.Core.ProductAggregate;
using OrderDemo.CleanArch.Core.ProductAggregate.Specifications;

namespace OrderDemo.CleanArch.UseCases.Cart.AddToCart;

public class AddToCartHandler(
  IRepository<Core.CartAggregate.Cart> cartRepository,
  IReadRepository<Product> productRepository)
  : ICommandHandler<AddToCartCommand, Result<CartDto>>
{
  public async ValueTask<Result<CartDto>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
  {
    // Validate product exists
    var productSpec = new ProductByIdSpec(ProductId.From(request.ProductId));
    var product = await productRepository.FirstOrDefaultAsync(productSpec, cancellationToken);
    if (product == null)
    {
      return Result.NotFound("Product not found");
    }

    // Get or create cart
    Core.CartAggregate.Cart cart;
    if (request.CartId.HasValue)
    {
      var cartSpec = new CartByIdSpec(request.CartId.Value);
      var existingCart = await cartRepository.FirstOrDefaultAsync(cartSpec, cancellationToken);
      if (existingCart == null)
      {
        return Result.NotFound("Cart not found");
      }
      cart = existingCart;
    }
    else
    {
      // Create new cart - AddAsync saves by default
      cart = new Core.CartAggregate.Cart();
      cart = await cartRepository.AddAsync(cart, cancellationToken);
    }

    // Add item to cart
    cart.AddItem(request.ProductId, request.Quantity, product.UnitPrice);

    // Update the cart with the new items
    await cartRepository.UpdateAsync(cart, cancellationToken);

    // Map to DTO
    var items = cart.Items.Select(i => new CartItemDto(
      i.ProductId,
      i.Quantity,
      i.UnitPrice,
      i.Quantity * i.UnitPrice
    )).ToList();

    var total = items.Sum(i => i.TotalPrice);

    return new CartDto(cart.Id, items, total);
  }
}
