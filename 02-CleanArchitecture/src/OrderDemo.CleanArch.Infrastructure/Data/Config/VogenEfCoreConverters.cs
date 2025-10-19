using OrderDemo.CleanArch.Core.ContributorAggregate;
using OrderDemo.CleanArch.Core.ProductAggregate;
using Vogen;

namespace OrderDemo.CleanArch.Infrastructure.Data.Config;

[EfCoreConverter<ContributorId>]
[EfCoreConverter<ContributorName>]
[EfCoreConverter<ProductId>]
internal partial class VogenEfCoreConverters;
