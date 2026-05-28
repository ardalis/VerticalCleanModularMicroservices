namespace Nimble.Modulith.Reporting.Services;

public interface IReportService
{
    Task<OrdersReportData> GetOrdersReportAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default);
    Task<ProductSalesReportData> GetProductSalesReportAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default);
    Task<CustomerOrdersReportData> GetCustomerOrdersReportAsync(int customerId, CancellationToken ct = default);
}
