using OrderDemo.CleanVertical.Web.Domain.CartAggregate;
using OrderDemo.CleanVertical.Web.Domain.GuestUserAggregate;
using OrderDemo.CleanVertical.Web.Domain.OrderAggregate;
using OrderDemo.CleanVertical.Web.Domain.ProductAggregate;
using Vogen;

namespace OrderDemo.CleanVertical.Web.Infrastructure.Data.Config;

[EfCoreConverter<ProductId>]
[EfCoreConverter<CartId>]
[EfCoreConverter<CartItemId>]
[EfCoreConverter<GuestUserId>]
[EfCoreConverter<OrderId>]
[EfCoreConverter<OrderItemId>]
[EfCoreConverter<Quantity>]
[EfCoreConverter<Price>]
internal partial class VogenEfCoreConverters;
