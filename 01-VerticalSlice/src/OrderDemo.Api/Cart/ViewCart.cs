using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderDemo.Api.Data;

namespace OrderDemo.Api.Cart;

public static class ViewCart
{
    public static void MapViewCartEndpoint(this WebApplication app)
    {
        app.MapGet("/cart/{orderId}", async (Guid orderId, AppDbContext dbContext) =>
        {
            var order = await dbContext.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == orderId)
                .Select(o => new
                {
                    o.Id,
                    o.CreatedOn,
                    Items = o.Items.Select(i => new
                    {
                        i.ProductId,
                        i.Product!.Name,
                        i.Quantity,
                        i.UnitPrice,
                        TotalPrice = i.Quantity * i.UnitPrice
                    }),
                    Total = o.Items.Sum(i => i.Quantity * i.UnitPrice)
                })
                .FirstOrDefaultAsync();

            return order is not null ? Results.Ok(order) : Results.NotFound("Cart not found");
        })
        .WithName("ViewCart")
        .WithTags("Cart");
    }
}