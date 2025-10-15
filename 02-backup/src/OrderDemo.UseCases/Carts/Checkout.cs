using Mediator;
using Microsoft.EntityFrameworkCore;
using OrderDemo.Core.GuestUserAggregate;
using OrderDemo.Core.OrderAggregate;
using OrderDemo.Infrastructure.Data;

namespace OrderDemo.UseCases.Carts;

public record CheckoutCommand(Guid CartId, Guid GuestUserId, string Email) : ICommand<CheckoutResultDto>;

public record CheckoutResultDto(Guid OrderId, string Message);

public sealed class CheckoutHandler(AppDbContext dbContext)
    : ICommandHandler<CheckoutCommand, CheckoutResultDto>
{
    public async ValueTask<CheckoutResultDto> Handle(CheckoutCommand command, CancellationToken cancellationToken)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == command.CartId && !c.Deleted, cancellationToken);

        if (cart is null)
        {
            throw new InvalidOperationException("Cart not found");
        }

        var guestUser = await dbContext.GuestUsers
            .FirstOrDefaultAsync(g => g.Id == command.GuestUserId, cancellationToken);

        if (guestUser is null)
        {
            guestUser = new GuestUser { Id = command.GuestUserId, Email = command.Email };
            dbContext.GuestUsers.Add(guestUser);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            GuestUserId = guestUser.Id
        };

        order.Items.AddRange(cart.Items.Select(i => new OrderItem
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }));

        dbContext.Orders.Add(order);
        cart.Deleted = true;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CheckoutResultDto(order.Id, "Checkout successful");
    }
}
