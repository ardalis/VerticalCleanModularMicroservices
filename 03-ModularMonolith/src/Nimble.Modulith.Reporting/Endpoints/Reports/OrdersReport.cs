using FastEndpoints;
using Nimble.Modulith.Reporting.Services;

namespace Nimble.Modulith.Reporting.Endpoints.Reports;

public class OrdersReportRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Format { get; set; } // Query parameter for format (json or csv)
}

public class OrdersReport(IReportService reportService) : Endpoint<OrdersReportRequest, OrdersReportData>
{
    public override void Configure()
    {
        Get("/reports/orders");
        AllowAnonymous(); // TODO: Add proper authorization
        Summary(s =>
        {
            s.Summary = "Get orders report for a date range";
            s.Description = "Returns order statistics for the specified date range. Supports JSON (default) and CSV formats.";
            s.Params["StartDate"] = "Start date for the report (YYYY-MM-DD)";
            s.Params["EndDate"] = "End date for the report (YYYY-MM-DD)";
            s.Params["Format"] = "Output format: 'json' (default) or 'csv'";
        });
        Tags("reports");
    }

    public override async Task HandleAsync(OrdersReportRequest req, CancellationToken ct)
    {
        // Validate dates
        if (req.EndDate < req.StartDate)
        {
            AddError("EndDate must be greater than or equal to StartDate");
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var reportData = await reportService.GetOrdersReportAsync(req.StartDate, req.EndDate, ct);

        // Determine output format from query string or Accept header
        var format = req.Format?.ToLower();
        var acceptHeader = HttpContext.Request.Headers.Accept.ToString().ToLower();

        if (format == "csv" || acceptHeader.Contains("text/csv"))
        {
            var csv = CsvFormatter.ToCsv(reportData.Orders);
            await Send.StringAsync(csv, contentType: "text/csv", cancellation: ct);
        }
        else
        {
            // Default to JSON
            Response = reportData;
        }
    }
}
