using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nimble.Modulith.Customers.Contracts;
using Nimble.Modulith.Reporting.Data;
using Nimble.Modulith.Reporting.Models;

namespace Nimble.Modulith.Reporting.Ingest;

/// <summary>
/// Handles OrderCreatedEvent from Customers module and inserts data into reporting star schema
/// </summary>
public class OrderCreatedEventHandler(
    ReportingDbContext dbContext,
    ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
{
    public async ValueTask Handle(OrderCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Ingesting order {OrderId} into reporting database", notification.OrderId);

        try
        {
            // Ensure dimension tables have the required data
            await EnsureDateDimensionAsync(notification.OrderDate, ct);
            await EnsureCustomerDimensionAsync(notification.CustomerId, notification.CustomerEmail, ct);

            // Insert fact records for each order item
            foreach (var item in notification.Items)
            {
                await EnsureProductDimensionAsync(item.ProductId, item.ProductName, ct);

                // Check if this order item already exists (idempotency)
                var exists = await dbContext.FactOrders
                    .AnyAsync(f => f.OrderId == notification.OrderId && f.OrderItemId == item.Id, ct);

                if (exists)
                {
                    logger.LogWarning("Order item {OrderId}/{OrderItemId} already exists in reporting database, skipping",
                        notification.OrderId, item.Id);
                    continue;
                }

                var factOrder = new FactOrder
                {
                    DateKey = ConvertToDateKey(notification.OrderDate),
                    CustomerId = notification.CustomerId,
                    ProductId = item.ProductId,
                    OrderId = notification.OrderId,
                    OrderNumber = notification.OrderNumber,
                    OrderItemId = item.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice,
                    OrderTotalAmount = notification.TotalAmount,
                    IngestedAt = DateTime.UtcNow
                };

                dbContext.FactOrders.Add(factOrder);
            }

            await dbContext.SaveChangesAsync(ct);

            logger.LogInformation("Successfully ingested order {OrderId} with {ItemCount} items",
                notification.OrderId, notification.Items.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to ingest order {OrderId} into reporting database", notification.OrderId);
            throw;
        }
    }

    private async Task EnsureDateDimensionAsync(DateOnly orderDate, CancellationToken ct)
    {
        var dateKey = ConvertToDateKey(orderDate);
        var exists = await dbContext.DimDates.AnyAsync(d => d.DateKey == dateKey, ct);

        if (!exists)
        {
            var date = orderDate.ToDateTime(TimeOnly.MinValue);
            var dimDate = new DimDate
            {
                DateKey = dateKey,
                Date = date,
                Year = date.Year,
                Quarter = (date.Month - 1) / 3 + 1,
                Month = date.Month,
                Day = date.Day,
                DayOfWeek = (int)date.DayOfWeek,
                DayName = date.DayOfWeek.ToString(),
                MonthName = date.ToString("MMMM")
            };

            dbContext.DimDates.Add(dimDate);
        }
    }

    private async Task EnsureCustomerDimensionAsync(int customerId, string customerEmail, CancellationToken ct)
    {
        var customer = await dbContext.DimCustomers.FindAsync(new object[] { customerId }, ct);

        if (customer == null)
        {
            customer = new DimCustomer
            {
                CustomerId = customerId,
                CustomerEmail = customerEmail,
                CustomerName = customerEmail, // We could enhance this with actual name
                FirstSeenDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };

            dbContext.DimCustomers.Add(customer);
        }
        else
        {
            customer.LastUpdatedDate = DateTime.UtcNow;
        }
    }

    private async Task EnsureProductDimensionAsync(int productId, string productName, CancellationToken ct)
    {
        var product = await dbContext.DimProducts.FindAsync(new object[] { productId }, ct);

        if (product == null)
        {
            product = new DimProduct
            {
                ProductId = productId,
                ProductName = productName,
                FirstSeenDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };

            dbContext.DimProducts.Add(product);
        }
        else
        {
            // Update product name if it changed
            if (product.ProductName != productName)
            {
                product.ProductName = productName;
            }
            product.LastUpdatedDate = DateTime.UtcNow;
        }
    }

    private static int ConvertToDateKey(DateOnly date)
    {
        return date.Year * 10000 + date.Month * 100 + date.Day;
    }
}
