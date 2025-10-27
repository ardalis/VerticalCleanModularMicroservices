namespace Nimble.Modulith.Customers.UseCases.Orders;

public record OrderDto(
    int Id,
    int CustomerId,
    string OrderNumber,
    DateOnly OrderDate,
    string Status,
    decimal TotalAmount,
    List<OrderItemDto> Items,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record OrderItemDto(
    int Id,
    int ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);
