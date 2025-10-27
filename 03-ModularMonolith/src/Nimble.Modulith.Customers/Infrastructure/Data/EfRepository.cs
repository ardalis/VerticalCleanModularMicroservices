using Ardalis.Specification.EntityFrameworkCore;
using Nimble.Modulith.Customers.Domain.Interfaces;

namespace Nimble.Modulith.Customers.Infrastructure.Data;

public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public EfRepository(CustomersDbContext dbContext) : base(dbContext)
    {
    }
}
