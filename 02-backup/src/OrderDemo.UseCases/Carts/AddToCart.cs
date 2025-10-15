using Mediator;
using Microsoft.EntityFrameworkCore;
using OrderDemo.Core.CartAggregate;
using OrderDemo.Infrastructure.Data;

namespace OrderDemo.UseCases.Carts;

public record AddToCartCommand(Guid CartId, int ProductId, int Quantity) : ICommand<CartDto>;

public record CartDto(Guid Id, List<CartItemDto> Items);

public record CartItemDto(int ProductId, int Quantity, decimal UnitPrice);

public sealed class AddToCartHandler(AppDbContext dbContext)
    : ICommandHandler<AddToCartCommand, CartDto>
{
    public async ValueTask<CartDto> Handle(AddToCartCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductId], cancellationToken);
        if (product is null)
        {
            throw new InvalidOperationException("Product not found");
        }

        var cart = await dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == command.CartId && !c.Deleted, cancellationToken);

        if (cart is null)
        {
            cart = new Cart { Id = command.CartId };
            dbContext.Carts.Add(cart);
        }

        cart.Items.Add(new CartItem
        {
            ProductId = product.Id,
            Quantity = command.Quantity,
            UnitPrice = product.UnitPrice
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new CartDto(
            cart.Id,
            cart.Items.Select(i => new CartItemDto(i.ProductId, i.Quantity, i.UnitPrice)).ToList()
        );

        return response;
    }
}
