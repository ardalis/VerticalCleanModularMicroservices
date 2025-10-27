using Mediator;

namespace Nimble.Modulith.Customers.Contracts;

public record OrderCreatedEvent(
    int OrderId,
    int CustomerId,
    string CustomerEmail,
    string OrderNumber,
    DateOnly OrderDate,
    decimal TotalAmount,
    List<OrderItemDetails> Items
) : INotification;
