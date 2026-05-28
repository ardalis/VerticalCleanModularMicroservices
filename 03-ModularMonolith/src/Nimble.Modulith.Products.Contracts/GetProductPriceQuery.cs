using Mediator;

namespace Nimble.Modulith.Products.Contracts;

public record GetProductPriceQuery(int ProductId) : IQuery<decimal>;
