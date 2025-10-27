using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Nimble.Modulith.Products.Data;

namespace Nimble.Modulith.Products.Endpoints;

public class DeleteProductRequest
{
    public int Id { get; set; }
}

public class Delete(ProductsDbContext dbContext) : Endpoint<DeleteProductRequest>
{
    private readonly ProductsDbContext _dbContext = dbContext;

    public override void Configure()
    {
        Delete("/products/{id}");
        Tags("products");
        Roles("Admin");
        Summary(s =>
        {
            s.Summary = "Delete a product";
            s.Description = "Deletes a product by its ID";
        });
    }

    public override async Task HandleAsync(DeleteProductRequest req, CancellationToken ct)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

        if (product is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}
