using Mediator;
using Microsoft.EntityFrameworkCore;
using OrderDemo.Infrastructure.Data;

namespace OrderDemo.UseCases.Products;

public record GetProductByIdQuery(int ProductId) : IQuery<ProductDetailDto?>;

public record ProductDetailDto(int Id, string Name, decimal UnitPrice);

public sealed class GetProductByIdHandler(AppDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, ProductDetailDto?>
{
    public async ValueTask<ProductDetailDto?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Where(p => p.Id == query.ProductId)
            .Select(p => new ProductDetailDto(p.Id, p.Name, p.UnitPrice))
            .FirstOrDefaultAsync(cancellationToken);

        return product;
    }
}
