using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Nimble.Modulith.Products.Data;

namespace Nimble.Modulith.Products.Endpoints;

public class GetByIdRequest
{
    public int Id { get; set; }
}

public class GetByIdResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime DateCreated { get; set; }
    public string CreatedByUser { get; set; } = string.Empty;
}

public class GetById(ProductsDbContext dbContext) : Endpoint<GetByIdRequest, GetByIdResponse>
{
    private readonly ProductsDbContext _dbContext = dbContext;

    public override void Configure()
    {
        Get("/products/{id}");
        Tags("products");
        Summary(s =>
        {
            s.Summary = "Get a product by ID";
            s.Description = "Retrieves a single product by its ID";
        });
    }

    public override async Task HandleAsync(GetByIdRequest req, CancellationToken ct)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

        if (product is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Response = new GetByIdResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            DateCreated = product.DateCreated,
            CreatedByUser = product.CreatedByUser
        };
    }
}
