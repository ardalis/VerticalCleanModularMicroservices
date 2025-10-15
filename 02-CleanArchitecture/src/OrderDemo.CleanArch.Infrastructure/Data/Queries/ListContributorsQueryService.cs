using OrderDemo.CleanArch.Core.ContributorAggregate;
using OrderDemo.CleanArch.UseCases.Contributors;
using OrderDemo.CleanArch.UseCases.Contributors.List;

namespace OrderDemo.CleanArch.Infrastructure.Data.Queries;

public class ListContributorsQueryService : IListContributorsQueryService
{
  // You can use EF, Dapper, SqlClient, etc. for queries
  private readonly AppDbContext _db;

  public ListContributorsQueryService(AppDbContext db)
  {
    _db = db;
  }

  public async Task<UseCases.PagedResult<ContributorDto>> ListAsync(int page, int perPage)
  {
    var items = await _db.Contributors.FromSqlRaw("SELECT Id, Name, PhoneNumber_CountryCode, PhoneNumber_Number, PhoneNumber_Extension FROM Contributors") // don't fetch other big columns
      .OrderBy(c => c.Id)
      .Skip((page - 1) * perPage)
      .Take(perPage)
      .Select(c => new ContributorDto(c.Id, c.Name, c.PhoneNumber ?? PhoneNumber.Unknown))
      .AsNoTracking()
      .ToListAsync();

    int totalCount = await _db.Contributors.CountAsync();
    int totalPages = (int)Math.Ceiling(totalCount / (double)perPage);
    var result = new UseCases.PagedResult<ContributorDto>(items, page, perPage, totalCount, totalPages);

    return result;
  }
}
