using OrderDemo.CleanArch.Core.CartAggregate;
using OrderDemo.CleanArch.Core.ContributorAggregate;
using OrderDemo.CleanArch.Core.ProductAggregate;
using Vogen;

namespace OrderDemo.CleanArch.Infrastructure.Data.Config;

[EfCoreConverter<ContributorId>]
[EfCoreConverter<ContributorName>]
[EfCoreConverter<ProductId>]
[EfCoreConverter<CartId>]
internal partial class VogenEfCoreConverters;
