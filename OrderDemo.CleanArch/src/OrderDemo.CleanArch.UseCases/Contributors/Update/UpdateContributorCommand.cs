using OrderDemo.CleanArch.Core.ContributorAggregate;

namespace OrderDemo.CleanArch.UseCases.Contributors.Update;

public record UpdateContributorCommand(ContributorId ContributorId, ContributorName NewName) : ICommand<Result<ContributorDto>>;
