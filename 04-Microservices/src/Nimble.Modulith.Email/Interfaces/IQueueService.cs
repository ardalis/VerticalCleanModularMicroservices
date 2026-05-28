namespace Nimble.Modulith.Email.Interfaces;

public interface IQueueService<T>
{
  ValueTask EnqueueAsync(T item, CancellationToken cancellationToken = default);
  ValueTask<T> DequeueAsync(CancellationToken cancellationToken = default);
}
