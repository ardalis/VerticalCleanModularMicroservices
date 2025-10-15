using Mediator;
using Microsoft.EntityFrameworkCore;
using OrderDemo.Infrastructure.Data;

namespace OrderDemo.UseCases.Orders;

public record GetOrdersQuery(string EmailAddress) : IQuery<IReadOnlyList<OrderSummaryDto>>;

public record OrderSummaryDto(
    Guid Id,
    string CreatedOn,
    decimal Total,
    int ItemCount,
    string Status,
    string DatePaid,
    string PaymentReference);

public sealed class GetOrdersHandler(AppDbContext dbContext)
    : IQueryHandler<GetOrdersQuery, IReadOnlyList<OrderSummaryDto>>
{
    public async ValueTask<IReadOnlyList<OrderSummaryDto>> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(o => o.Items)
            .Include(o => o.GuestUser)
            .Where(o => o.GuestUser != null && o.GuestUser.Email == query.EmailAddress)
            .OrderByDescending(o => o.CreatedOn)
            .Select(o => new OrderSummaryDto(
                o.Id,
                o.CreatedOn.ToString(),
                o.Items.Sum(i => i.UnitPrice * i.Quantity),
                o.Items.Count,
                o.DatePaid.HasValue ? "Paid" : "Pending Payment",
                o.DatePaid.HasValue ? o.DatePaid.ToString() : "N/A",
                o.PaymentReference
            ))
            .ToListAsync(cancellationToken);

        return orders;
    }
}
