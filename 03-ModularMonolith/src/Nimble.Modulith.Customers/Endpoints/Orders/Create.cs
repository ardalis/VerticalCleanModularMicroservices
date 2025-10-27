using FastEndpoints;
using Mediator;
using Nimble.Modulith.Customers.Infrastructure;
using Nimble.Modulith.Customers.UseCases.Customers.Queries;
using Nimble.Modulith.Customers.UseCases.Orders.Commands;

namespace Nimble.Modulith.Customers.Endpoints.Orders;

public class Create(IMediator mediator, ICustomerAuthorizationService authService) 
: Endpoint<CreateOrderRequest, OrderResponse>
{
    public override void Configure()
    {
        Post("/orders");
        // Require authentication - removed AllowAnonymous()
        Summary(s =>
        {
            s.Summary = "Create a new order";
            s.Description = "Creates a new order with the provided items";
        });
        Tags("orders");
    }

    public override async Task HandleAsync(CreateOrderRequest req, CancellationToken ct)
    {
        // Verify the customer exists and user has permission to create orders for them
        var customerQuery = new GetCustomerByIdQuery(req.CustomerId);
        var customerResult = await mediator.Send(customerQuery, ct);

        if (!customerResult.IsSuccess)
        {
            AddError($"Customer with ID {req.CustomerId} not found");
            await Send.ErrorsAsync(statusCode: 404, cancellation: ct);
            return;
        }

        // Check if user is Admin or creating order for their own customer record
        if (!authService.IsAdminOrOwner(User, customerResult.Value.Email))
        {
            AddError("You can only create orders for your own customer record");
            await Send.ErrorsAsync(statusCode: 403, cancellation: ct);
            return;
        }

        var command = new CreateOrderCommand(
            req.CustomerId,
            req.OrderDate,
            req.Items.Select(i => new CreateOrderItemDto(
                i.ProductId,
                i.Quantity
            )).ToList()
        );

        var result = await mediator.Send(command, ct);

        if (!result.IsSuccess)
        {
            AddError("Failed to create order");
            await Send.ErrorsAsync(cancellation: ct);
            return;
        }

        // Map UseCases DTO to Endpoint Response DTO
        await Send.CreatedAtAsync<GetById>(
            new { id = result.Value.Id },
            new OrderResponse(
                result.Value.Id,
                result.Value.CustomerId,
                result.Value.OrderNumber,
                result.Value.OrderDate,
                result.Value.Status,
                result.Value.TotalAmount,
                result.Value.Items.Select(i => new OrderItemResponse(
                    i.Id,
                    i.ProductId,
                    i.ProductName,
                    i.Quantity,
                    i.UnitPrice,
                    i.TotalPrice
                )).ToList()
            ),
            generateAbsoluteUrl: false,
            cancellation: ct
        );
    }
}
