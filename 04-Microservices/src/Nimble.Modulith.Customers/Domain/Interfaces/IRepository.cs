using Ardalis.Specification;

namespace Nimble.Modulith.Customers.Domain.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}
