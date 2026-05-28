using Ardalis.Result;
using FastEndpoints;
using Mediator;
using Nimble.Modulith.Customers.Infrastructure;
using Nimble.Modulith.Customers.UseCases.Customers.Queries;

namespace Nimble.Modulith.Customers.Endpoints.Customers;

public class GetById(IMediator mediator, ICustomerAuthorizationService authService) : EndpointWithoutRequest<CustomerResponse>
{
    public override void Configure()
    {
        Get("/customers/{id}");
        // Require authentication - removed AllowAnonymous()
        Summary(s =>
        {
            s.Summary = "Get a customer by ID";
            s.Description = "Returns a single customer by their ID";
        });
        Tags("customers");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var query = new GetCustomerByIdQuery(id);
        var result = await mediator.Send(query, ct);

        if (!result.IsSuccess)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        // Check if user is Admin or viewing their own customer record
        if (!authService.IsAdminOrOwner(User, result.Value.Email))
        {
            AddError("You can only view your own customer record");
            await Send.ErrorsAsync(statusCode: 403, cancellation: ct);
            return;
        }

        // Map UseCases DTO to Endpoint Response DTO
        Response = new CustomerResponse(
            result.Value.Id,
            result.Value.FirstName,
            result.Value.LastName,
            result.Value.Email,
            result.Value.PhoneNumber,
            new AddressResponse(
                result.Value.Address.Street,
                result.Value.Address.City,
                result.Value.Address.State,
                result.Value.Address.PostalCode,
                result.Value.Address.Country
            )
        );
    }
}
