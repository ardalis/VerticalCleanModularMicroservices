using OrderDemo.CleanArch.Core.ContributorAggregate;

namespace OrderDemo.CleanArch.UseCases.Contributors;
public record ContributorDto(ContributorId Id, ContributorName Name, PhoneNumber PhoneNumber);
