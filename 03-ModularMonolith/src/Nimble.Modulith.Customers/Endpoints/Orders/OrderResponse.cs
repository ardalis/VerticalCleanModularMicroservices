namespace Nimble.Modulith.Customers.Endpoints.Orders;

public record OrderResponse(
    int Id,
    int CustomerId,
    string OrderNumber,
    DateOnly OrderDate,
    string Status,
    decimal TotalAmount,
    List<OrderItemResponse> Items
);

public record OrderItemResponse(
    int Id,
    int ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);

public record CreateOrderRequest(
    int CustomerId,
    DateOnly OrderDate,
    List<CreateOrderItemRequest> Items
);

public record CreateOrderItemRequest(
    int ProductId,
    string ProductName,
    int Quantity
);

public record AddOrderItemRequest(
    int ProductId,
    string ProductName,
    int Quantity
);
