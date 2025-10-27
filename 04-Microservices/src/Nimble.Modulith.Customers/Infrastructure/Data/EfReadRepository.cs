using Ardalis.Specification.EntityFrameworkCore;
using Nimble.Modulith.Customers.Domain.Interfaces;

namespace Nimble.Modulith.Customers.Infrastructure.Data;

public class EfReadRepository<T> : RepositoryBase<T>, IReadRepository<T> where T : class
{
    public EfReadRepository(CustomersDbContext dbContext) : base(dbContext)
    {
    }
}
