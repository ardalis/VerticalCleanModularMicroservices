using Ardalis.Result;
using Mediator;
using Nimble.Modulith.Customers.Domain.CustomerAggregate;
using Nimble.Modulith.Customers.Domain.Interfaces;
using Nimble.Modulith.Customers.Domain.OrderAggregate;
using Nimble.Modulith.Products.Contracts;

namespace Nimble.Modulith.Customers.UseCases.Orders.Commands;

public class CreateOrderHandler(
    IRepository<Order> orderRepository,
    IReadRepository<Customer> customerRepository,
    IMediator mediator) 
    : ICommandHandler<CreateOrderCommand, Result<OrderDto>>
{
    public async ValueTask<Result<OrderDto>> Handle(CreateOrderCommand command, CancellationToken ct)
    {
        // Verify customer exists
        var customer = await customerRepository.GetByIdAsync(command.CustomerId, ct);
        if (customer is null)
        {
            return Result<OrderDto>.NotFound($"Customer with ID {command.CustomerId} not found");
        }

        // Create order entity
        var order = new Order
        {
            CustomerId = command.CustomerId,
            OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}",
            OrderDate = command.OrderDate,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        // Add order items - fetch product details from Products module
        foreach (var itemDto in command.Items)
        {
            // Fetch the product details (name and price) from the Products module
            ProductDetailsResult productDetails;
            try
            {
                productDetails = await mediator.Send(new GetProductDetailsQuery(itemDto.ProductId), ct);
            }
            catch (InvalidOperationException ex)
            {
                return Result<OrderDto>.Error($"Failed to get product details for product {itemDto.ProductId}: {ex.Message}");
            }

            var item = new OrderItem
            {
                ProductId = itemDto.ProductId,
                ProductName = productDetails.Name,
                Quantity = itemDto.Quantity,
                UnitPrice = productDetails.Price
            };
            order.AddItem(item);
        }

        await orderRepository.AddAsync(order, ct);
        await orderRepository.SaveChangesAsync(ct);

        // Map to DTO
        var dto = new OrderDto(
            order.Id,
            order.CustomerId,
            order.OrderNumber,
            order.OrderDate,
            order.Status.ToString(),
            order.TotalAmount,
            order.Items.Select(i => new OrderItemDto(
                i.Id,
                i.ProductId,
                i.ProductName,
                i.Quantity,
                i.UnitPrice,
                i.TotalPrice
            )).ToList(),
            order.CreatedAt,
            order.UpdatedAt
        );

        return Result<OrderDto>.Success(dto);
    }
}
