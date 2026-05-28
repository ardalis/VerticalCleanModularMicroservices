namespace Nimble.Modulith.Reporting.Models;

/// <summary>
/// Customer dimension for star schema
/// </summary>
public class DimCustomer
{
    public int CustomerId { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime FirstSeenDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    
    // Navigation
    public ICollection<FactOrder> Orders { get; set; } = new List<FactOrder>();
}
