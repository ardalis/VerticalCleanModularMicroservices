using OrderDemo.CleanArch.Core.CartAggregate;
using OrderDemo.CleanArch.UseCases.Cart.Checkout;
using OrderDemo.CleanArch.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace OrderDemo.CleanArch.Web.Cart;

public class Checkout(IMediator mediator)
  : Endpoint<CheckoutRequest,
             Results<Ok<CheckoutResponse>,
                     NotFound,
                     ValidationProblem,
                     ProblemHttpResult>,
             CheckoutMapper>
{
  public override void Configure()
  {
    Post(CheckoutRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Checkout cart";
      s.Description = "Processes checkout for a cart, creating an order and marking the cart as completed. Requires a guest email address.";
      s.ExampleRequest = new CheckoutRequest 
      { 
        CartId = Guid.Parse("12345678-1234-1234-1234-123456789012"),
        Email = "guest@example.com"
      };
      s.ResponseExamples[200] = new CheckoutResponse(
        Guid.Parse("87654321-4321-4321-4321-210987654321"));

      // Document possible responses
      s.Responses[200] = "Checkout completed successfully";
      s.Responses[404] = "Cart not found";
      s.Responses[400] = "Invalid request data or empty cart";
    });

    // Add tags for API grouping
    Tags("Cart");

    // Add additional metadata
    Description(builder => builder
      .Accepts<CheckoutRequest>("application/json")
      .Produces<CheckoutResponse>(200, "application/json")
      .ProducesProblem(404)
      .ProducesProblem(400));
  }

  public override async Task<Results<Ok<CheckoutResponse>, NotFound, ValidationProblem, ProblemHttpResult>>
    ExecuteAsync(CheckoutRequest request, CancellationToken ct)
  {
    var cartId = CartId.From(request.CartId);
    var command = new CheckoutCommand(cartId, request.Email);
    var result = await mediator.Send(command, ct);

    if (result.Status == ResultStatus.NotFound)
    {
      return TypedResults.NotFound();
    }

    if (result.Status == ResultStatus.Invalid)
    {
      return TypedResults.ValidationProblem(result.ValidationErrors.ToDictionary(e => e.Identifier, e => new[] { e.ErrorMessage }));
    }

    if (!result.IsSuccess)
    {
      return TypedResults.Problem(result.Errors.FirstOrDefault() ?? "An error occurred during checkout");
    }

    var response = Map.FromEntity(result.Value);
    return TypedResults.Ok(response);
  }
}

public sealed class CheckoutMapper
  : Mapper<CheckoutRequest, CheckoutResponse, CheckoutResult>
{
  public override CheckoutResponse FromEntity(CheckoutResult e)
  {
    return new CheckoutResponse(e.OrderId.Value);
  }
}