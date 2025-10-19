﻿using OrderDemo.CleanArch.Core.ProductAggregate;
using OrderDemo.CleanArch.UseCases.Products;
using OrderDemo.CleanArch.UseCases.Products.Get;
using OrderDemo.CleanArch.Web.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace OrderDemo.CleanArch.Web.Products;

public class GetById(IMediator mediator)
  : Endpoint<GetProductByIdRequest,
             Results<Ok<ProductRecord>,
                     NotFound,
                     ProblemHttpResult>,
             GetProductByIdMapper>
{
  public override void Configure()
  {
    Get(GetProductByIdRequest.Route);
    AllowAnonymous();

    Summary(s =>
    {
      s.Summary = "Get a product by ID";
      s.Description = "Retrieves a specific product by its unique identifier. Returns detailed product information including ID, name, and unit price.";
      s.ExampleRequest = new GetProductByIdRequest { ProductId = 1 };
      s.ResponseExamples[200] = new ProductRecord(1, "Laptop", 999.99m);

      // Document possible responses
      s.Responses[200] = "Product found and returned successfully";
      s.Responses[404] = "Product with specified ID not found";
    });

    // Add tags for API grouping
    Tags("Products");

    // Add additional metadata
    Description(builder => builder
      .Accepts<GetProductByIdRequest>()
      .Produces<ProductRecord>(200, "application/json")
      .ProducesProblem(404));
  }

  public override async Task<Results<Ok<ProductRecord>, NotFound, ProblemHttpResult>>
    ExecuteAsync(GetProductByIdRequest request, CancellationToken ct)
  {
    var result = await mediator.Send(new GetProductQuery(ProductId.From(request.ProductId)), ct);

    return result.ToGetByIdResult(Map.FromEntity);
  }
}

public sealed class GetProductByIdMapper
  : Mapper<GetProductByIdRequest, ProductRecord, ProductDto>
{
  public override ProductRecord FromEntity(ProductDto e)
    => new(e.Id.Value, e.Name, e.UnitPrice);
}
