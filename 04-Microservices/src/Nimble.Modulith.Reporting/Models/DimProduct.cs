namespace Nimble.Modulith.Reporting.Models;

/// <summary>
/// Product dimension for star schema
/// </summary>
public class DimProduct
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public DateTime FirstSeenDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    
    // Navigation
    public ICollection<FactOrder> OrderItems { get; set; } = new List<FactOrder>();
}
