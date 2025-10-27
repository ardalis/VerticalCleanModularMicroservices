using FastEndpoints;
using Nimble.Modulith.Reporting.Services;

namespace Nimble.Modulith.Reporting.Endpoints.Reports;

public class ProductSalesReportRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Format { get; set; }
}

public class ProductSalesReport(IReportService reportService) : Endpoint<ProductSalesReportRequest, ProductSalesReportData>
{
    public override void Configure()
    {
        Get("/reports/product-sales");
        AllowAnonymous(); // TODO: Add proper authorization
        Summary(s =>
        {
            s.Summary = "Get product sales report for a date range";
            s.Description = "Returns product sales statistics for the specified date range. Supports JSON (default) and CSV formats.";
            s.Params["StartDate"] = "Start date for the report (YYYY-MM-DD)";
            s.Params["EndDate"] = "End date for the report (YYYY-MM-DD)";
            s.Params["Format"] = "Output format: 'json' (default) or 'csv'";
        });
        Tags("reports");
    }

    public override async Task HandleAsync(ProductSalesReportRequest req, CancellationToken ct)
    {
        if (req.EndDate < req.StartDate)
        {
            AddError("EndDate must be greater than or equal to StartDate");
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        var reportData = await reportService.GetProductSalesReportAsync(req.StartDate, req.EndDate, ct);

        var format = req.Format?.ToLower();
        var acceptHeader = HttpContext.Request.Headers.Accept.ToString().ToLower();

        if (format == "csv" || acceptHeader.Contains("text/csv"))
        {
            var csv = CsvFormatter.ToCsv(reportData.Products);
            await Send.StringAsync(csv, contentType: "text/csv", cancellation: ct);
        }
        else
        {
            Response = reportData;
        }
    }
}
