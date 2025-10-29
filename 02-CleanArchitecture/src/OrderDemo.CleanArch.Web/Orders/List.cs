using OrderDemo.CleanArch.UseCases.Orders;
using OrderDemo.CleanArch.UseCases.Orders.List;

namespace OrderDemo.CleanArch.Web.Orders;

public class List(IMediator mediator) : EndpointWithoutRequest<List<OrderDto>>
{
  private readonly IMediator _mediator = mediator;

  public override void Configure()
  {
    Get("/Orders");
    AllowAnonymous();
    Tags("Orders");

  }

  public override async Task HandleAsync(CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new ListOrdersQuery());
    if (!result.IsSuccess)
    {
      await Send.ErrorsAsync(statusCode: 400, cancellationToken);
      return;
    }

    var response = result.Value;

    await Send.OkAsync(response, cancellationToken);
  }

}


