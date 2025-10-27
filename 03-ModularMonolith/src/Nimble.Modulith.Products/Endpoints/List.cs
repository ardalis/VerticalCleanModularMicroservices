using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Nimble.Modulith.Products.Data;

namespace Nimble.Modulith.Products.Endpoints;

public class ProductListItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime DateCreated { get; set; }
    public string CreatedByUser { get; set; } = string.Empty;
}

public class ListResponse
{
    public List<ProductListItem> Products { get; set; } = new();
}

public class List(ProductsDbContext dbContext) : EndpointWithoutRequest<ListResponse>
{
    private readonly ProductsDbContext _dbContext = dbContext;

    public override void Configure()
    {
        Get("/products");
        Tags("products");
        Summary(s =>
        {
            s.Summary = "List all products";
            s.Description = "Retrieves a list of all products in the system";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var products = await _dbContext.Products
            .Select(p => new ProductListItem
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                DateCreated = p.DateCreated,
                CreatedByUser = p.CreatedByUser
            })
            .ToListAsync(ct);

        Response = new ListResponse
        {
            Products = products
        };
    }
}
