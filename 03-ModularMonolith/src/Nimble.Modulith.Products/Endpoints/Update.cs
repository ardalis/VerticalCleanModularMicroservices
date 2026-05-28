using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Nimble.Modulith.Products.Data;

namespace Nimble.Modulith.Products.Endpoints;

public class UpdateProductRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class UpdateProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime DateCreated { get; set; }
    public string CreatedByUser { get; set; } = string.Empty;
}

public class Update(ProductsDbContext dbContext) : Endpoint<UpdateProductRequest, UpdateProductResponse>
{
    private readonly ProductsDbContext _dbContext = dbContext;

    public override void Configure()
    {
        Put("/products/{id}");
        Tags("products");
        Roles("Admin");
        Summary(s =>
        {
            s.Summary = "Update a product";
            s.Description = "Updates an existing product's name and description";
        });
    }

    public override async Task HandleAsync(UpdateProductRequest req, CancellationToken ct)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

        if (product is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        product.Name = req.Name;
        product.Description = req.Description;
        product.Price = req.Price;

        await _dbContext.SaveChangesAsync(ct);

        Response = new UpdateProductResponse
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
