using Mediator;
using Microsoft.EntityFrameworkCore;
using OrderDemo.Infrastructure.Data;

namespace OrderDemo.UseCases.Products;

public record ListProductsQuery() : IQuery<IReadOnlyList<ProductDto>>;

public record ProductDto(int Id, string Name, decimal UnitPrice);

public sealed class ListProductsHandler(AppDbContext dbContext)
    : IQueryHandler<ListProductsQuery, IReadOnlyList<ProductDto>>
{
    public async ValueTask<IReadOnlyList<ProductDto>> Handle(ListProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await dbContext.Products
            .Select(p => new ProductDto(p.Id, p.Name, p.UnitPrice))
            .ToListAsync(cancellationToken);

        return products;
    }
}
