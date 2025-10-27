using FastEndpoints;
using Nimble.Modulith.Reporting.Services;

namespace Nimble.Modulith.Reporting.Endpoints.Reports;

public class CustomerOrdersReportRequest
{
    public int CustomerId { get; set; }
    public string? Format { get; set; }
}

public class CustomerOrdersReport(IReportService reportService) : Endpoint<CustomerOrdersReportRequest, CustomerOrdersReportData>
{
    public override void Configure()
    {
        Get("/reports/customers/{CustomerId}/orders");
        AllowAnonymous(); // TODO: Add proper authorization
        Summary(s =>
        {
            s.Summary = "Get orders report for a specific customer";
            s.Description = "Returns all orders and statistics for a specific customer. Supports JSON (default) and CSV formats.";
            s.Params["CustomerId"] = "The customer ID to get orders for";
            s.Params["Format"] = "Output format: 'json' (default) or 'csv'";
        });
        Tags("reports");
    }

    public override async Task HandleAsync(CustomerOrdersReportRequest req, CancellationToken ct)
    {
        try
        {
            var reportData = await reportService.GetCustomerOrdersReportAsync(req.CustomerId, ct);

            var format = req.Format?.ToLower();
            var acceptHeader = HttpContext.Request.Headers.Accept.ToString().ToLower();

            if (format == "csv" || acceptHeader.Contains("text/csv"))
            {
                var csv = CsvFormatter.ToCsv(reportData.Orders);
                await Send.StringAsync(csv, contentType: "text/csv", cancellation: ct);
            }
            else
            {
                Response = reportData;
            }
        }
        catch (InvalidOperationException ex)
        {
            AddError(ex.Message);
            await Send.ErrorsAsync(statusCode: 404, cancellation: ct);
        }
    }
}
