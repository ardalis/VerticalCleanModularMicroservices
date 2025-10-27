namespace Nimble.Modulith.Reporting.Models;

/// <summary>
/// Fact table for orders - one row per order item
/// This is the central fact table in the star schema
/// </summary>
public class FactOrder
{
    public long Id { get; set; }
    
    // Foreign keys to dimensions
    public int DateKey { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    
    // Order information (degenerate dimensions)
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    
    // Order item information (degenerate dimensions)
    public int OrderItemId { get; set; }
    
    // Measures/Facts
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal OrderTotalAmount { get; set; }
    
    // Audit
    public DateTime IngestedAt { get; set; }
    
    // Navigation properties
    public DimDate Date { get; set; } = null!;
    public DimCustomer Customer { get; set; } = null!;
    public DimProduct Product { get; set; } = null!;
}
