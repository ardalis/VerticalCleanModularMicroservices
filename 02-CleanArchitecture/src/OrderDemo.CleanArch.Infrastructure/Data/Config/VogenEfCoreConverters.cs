using OrderDemo.CleanArch.Core.CartAggregate;
using OrderDemo.CleanArch.Core.OrderAggregate;
using OrderDemo.CleanArch.Core.ProductAggregate;
using Vogen;

namespace OrderDemo.CleanArch.Infrastructure.Data.Config;

[EfCoreConverter<ProductId>]
[EfCoreConverter<CartId>]
[EfCoreConverter<CartItemId>]
[EfCoreConverter<OrderId>]
[EfCoreConverter<OrderItemId>]
[EfCoreConverter<Quantity>]
[EfCoreConverter<Price>]
internal partial class VogenEfCoreConverters;
