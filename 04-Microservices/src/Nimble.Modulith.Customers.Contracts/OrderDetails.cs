namespace Nimble.Modulith.Customers.Contracts;

/// <summary>
/// Order details for cross-module communication
/// </summary>
public record OrderDetails(
    int Id,
    int CustomerId,
    string OrderNumber,
    DateTime OrderDate,
    string Status,
    decimal TotalAmount,
    List<OrderItemDetails> Items
);

public record OrderItemDetails(
    int Id,
    int ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);
