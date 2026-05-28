using Ardalis.Specification;

namespace Nimble.Modulith.Customers.Domain.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
{
}
