namespace Nimble.Modulith.Reporting.Models;

/// <summary>
/// Date dimension for star schema - supports date-based analysis
/// </summary>
public class DimDate
{
    public int DateKey { get; set; } // YYYYMMDD format (e.g., 20250126)
    public DateTime Date { get; set; }
    public int Year { get; set; }
    public int Quarter { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public int DayOfWeek { get; set; }
    public string DayName { get; set; } = string.Empty;
    public string MonthName { get; set; } = string.Empty;
    
    // Navigation
    public ICollection<FactOrder> Orders { get; set; } = new List<FactOrder>();
}
