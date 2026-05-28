using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Nimble.Modulith.Reporting.Services;

public class ReportService : IReportService
{
    private readonly string _connectionString;

    public ReportService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("reportingdb")
            ?? throw new InvalidOperationException("Connection string 'reportingdb' not found");
    }

    public async Task<OrdersReportData> GetOrdersReportAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var startDateKey = ConvertToDateKey(DateOnly.FromDateTime(startDate));
        var endDateKey = ConvertToDateKey(DateOnly.FromDateTime(endDate));

        // Get order details
        var ordersSql = @"
            SELECT 
                f.OrderId,
                f.OrderNumber,
                d.Date as OrderDate,
                f.CustomerId,
                c.CustomerEmail,
                f.OrderTotalAmount as OrderTotal,
                COUNT(f.Id) as TotalItems
            FROM FactOrders f
            INNER JOIN DimDate d ON f.DateKey = d.DateKey
            INNER JOIN DimCustomer c ON f.CustomerId = c.CustomerId
            WHERE f.DateKey >= @StartDateKey AND f.DateKey <= @EndDateKey
            GROUP BY f.OrderId, f.OrderNumber, d.Date, f.CustomerId, c.CustomerEmail, f.OrderTotalAmount
            ORDER BY d.Date DESC, f.OrderId DESC";

        var orders = (await connection.QueryAsync<OrderReportItem>(ordersSql, new { StartDateKey = startDateKey, EndDateKey = endDateKey })).ToList();

        // Calculate summary
        var summary = new OrderReportSummary(
            TotalOrders: orders.Count,
            TotalRevenue: orders.Sum(o => o.OrderTotal),
            TotalItems: orders.Sum(o => o.TotalItems),
            AverageOrderValue: orders.Count > 0 ? orders.Average(o => o.OrderTotal) : 0
        );

        return new OrdersReportData(orders, summary);
    }

    public async Task<ProductSalesReportData> GetProductSalesReportAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        var startDateKey = ConvertToDateKey(DateOnly.FromDateTime(startDate));
        var endDateKey = ConvertToDateKey(DateOnly.FromDateTime(endDate));

        var sql = @"
            SELECT 
                p.ProductId,
                p.ProductName,
                SUM(f.Quantity) as TotalQuantitySold,
                SUM(f.TotalPrice) as TotalRevenue,
                COUNT(DISTINCT f.OrderId) as OrderCount
            FROM FactOrders f
            INNER JOIN DimProduct p ON f.ProductId = p.ProductId
            WHERE f.DateKey >= @StartDateKey AND f.DateKey <= @EndDateKey
            GROUP BY p.ProductId, p.ProductName
            ORDER BY TotalRevenue DESC";

        var products = (await connection.QueryAsync<ProductSalesItem>(sql, new { StartDateKey = startDateKey, EndDateKey = endDateKey })).ToList();

        var summary = new ProductSalesReportSummary(
            TotalProducts: products.Count,
            TotalQuantitySold: products.Sum(p => p.TotalQuantitySold),
            TotalRevenue: products.Sum(p => p.TotalRevenue)
        );

        return new ProductSalesReportData(products, summary);
    }

    public async Task<CustomerOrdersReportData> GetCustomerOrdersReportAsync(int customerId, CancellationToken ct = default)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(ct);

        // Get customer info
        var customerSql = "SELECT CustomerEmail FROM DimCustomer WHERE CustomerId = @CustomerId";
        var customerEmail = await connection.QuerySingleOrDefaultAsync<string>(customerSql, new { CustomerId = customerId });

        if (customerEmail == null)
        {
            throw new InvalidOperationException($"Customer with ID {customerId} not found in reporting database");
        }

        // Get customer orders
        var ordersSql = @"
            SELECT 
                f.OrderId,
                f.OrderNumber,
                d.Date as OrderDate,
                f.OrderTotalAmount as OrderTotal,
                COUNT(f.Id) as ItemCount
            FROM FactOrders f
            INNER JOIN DimDate d ON f.DateKey = d.DateKey
            WHERE f.CustomerId = @CustomerId
            GROUP BY f.OrderId, f.OrderNumber, d.Date, f.OrderTotalAmount
            ORDER BY d.Date DESC";

        var orders = (await connection.QueryAsync<CustomerOrderItem>(ordersSql, new { CustomerId = customerId })).ToList();

        var summary = new CustomerOrdersSummary(
            TotalOrders: orders.Count,
            TotalSpent: orders.Sum(o => o.OrderTotal),
            FirstOrderDate: orders.Count > 0 ? orders.Min(o => o.OrderDate) : DateTime.MinValue,
            LastOrderDate: orders.Count > 0 ? orders.Max(o => o.OrderDate) : DateTime.MinValue
        );

        return new CustomerOrdersReportData(customerId, customerEmail, orders, summary);
    }

    private static int ConvertToDateKey(DateOnly date)
    {
        return date.Year * 10000 + date.Month * 100 + date.Day;
    }
}
