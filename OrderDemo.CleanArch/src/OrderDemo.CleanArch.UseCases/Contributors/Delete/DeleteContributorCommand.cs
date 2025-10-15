using OrderDemo.CleanArch.Core.ContributorAggregate;

namespace OrderDemo.CleanArch.UseCases.Contributors.Delete;

public record DeleteContributorCommand(ContributorId ContributorId) : ICommand<Result>;
