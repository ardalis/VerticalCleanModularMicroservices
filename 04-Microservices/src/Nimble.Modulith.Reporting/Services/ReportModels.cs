namespace Nimble.Modulith.Reporting.Services;

/// <summary>
/// Report data for orders over a date range
/// </summary>
public record OrdersReportData(
    List<OrderReportItem> Orders,
    OrderReportSummary Summary
);

public record OrderReportItem(
    int OrderId,
    string OrderNumber,
    DateTime OrderDate,
    int CustomerId,
    string CustomerEmail,
    decimal OrderTotal,
    int TotalItems
);

public record OrderReportSummary(
    int TotalOrders,
    decimal TotalRevenue,
    int TotalItems,
    decimal AverageOrderValue
);

/// <summary>
/// Report data for product sales
/// </summary>
public record ProductSalesReportData(
    List<ProductSalesItem> Products,
    ProductSalesReportSummary Summary
);

public record ProductSalesItem(
    int ProductId,
    string ProductName,
    int TotalQuantitySold,
    decimal TotalRevenue,
    int OrderCount
);

public record ProductSalesReportSummary(
    int TotalProducts,
    int TotalQuantitySold,
    decimal TotalRevenue
);

/// <summary>
/// Report data for customer orders
/// </summary>
public record CustomerOrdersReportData(
    int CustomerId,
    string CustomerEmail,
    List<CustomerOrderItem> Orders,
    CustomerOrdersSummary Summary
);

public record CustomerOrderItem(
    int OrderId,
    string OrderNumber,
    DateTime OrderDate,
    decimal OrderTotal,
    int ItemCount
);

public record CustomerOrdersSummary(
    int TotalOrders,
    decimal TotalSpent,
    DateTime FirstOrderDate,
    DateTime LastOrderDate
);
