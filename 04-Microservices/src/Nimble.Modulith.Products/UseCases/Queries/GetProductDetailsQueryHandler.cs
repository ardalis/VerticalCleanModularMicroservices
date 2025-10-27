using Mediator;
using Microsoft.EntityFrameworkCore;
using Nimble.Modulith.Products.Contracts;
using Nimble.Modulith.Products.Data;

namespace Nimble.Modulith.Products.UseCases.Queries;

public class GetProductDetailsQueryHandler(ProductsDbContext dbContext)
    : IQueryHandler<GetProductDetailsQuery, ProductDetailsResult>
{
    public async ValueTask<ProductDetailsResult> Handle(GetProductDetailsQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == query.ProductId, cancellationToken);

        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {query.ProductId} not found");
        }

        return new ProductDetailsResult(product.Id, product.Name, product.Price);
    }
}
