using Mediator;
using Microsoft.EntityFrameworkCore;
using OrderDemo.Infrastructure.Data;

namespace OrderDemo.UseCases.Carts;

public record ViewCartQuery(Guid CartId) : IQuery<CartDto?>;

public sealed class ViewCartHandler(AppDbContext dbContext)
    : IQueryHandler<ViewCartQuery, CartDto?>
{
    public async ValueTask<CartDto?> Handle(ViewCartQuery query, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == query.CartId && !c.Deleted, cancellationToken);

        if (cart is null)
        {
            return null;
        }

        var response = new CartDto(
            cart.Id,
            cart.Items.Select(i => new CartItemDto(i.ProductId, i.Quantity, i.UnitPrice)).ToList()
        );

        return response;
    }
}
