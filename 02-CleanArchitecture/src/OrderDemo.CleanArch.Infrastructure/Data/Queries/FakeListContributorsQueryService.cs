using OrderDemo.CleanArch.Core.ContributorAggregate;
using OrderDemo.CleanArch.UseCases.Contributors;
using OrderDemo.CleanArch.UseCases.Contributors.List;

namespace OrderDemo.CleanArch.Infrastructure.Data.Queries;

public class FakeListContributorsQueryService : IListContributorsQueryService
{
  public Task<UseCases.PagedResult<ContributorDto>> ListAsync(int page, int perPage)
  {
    var items = new List<ContributorDto>();
    for (int i = 1; i <= 25; i++)
    {
      var phone = new PhoneNumber("+1", "555", "1234567");
      items.Add(new ContributorDto(ContributorId.From(i), ContributorName.From($"Fake {i}"), phone));
    }

    int totalPages = (int)Math.Ceiling(items.Count / (double)perPage);
    var result = new UseCases.PagedResult<ContributorDto>(items, page, perPage, items.Count, totalPages);
    return Task.FromResult(result);
  }
}
