using OrderDemo.CleanArch.Core.ContributorAggregate;

namespace OrderDemo.CleanArch.UseCases.Contributors.Get;

public record GetContributorQuery(ContributorId ContributorId) : IQuery<Result<ContributorDto>>;
