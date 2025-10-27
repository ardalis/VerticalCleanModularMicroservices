using System.Threading.Channels;
using Nimble.Modulith.Email.Interfaces;

namespace Nimble.Modulith.Email.Services;

public class ChannelQueueService<T> : IQueueService<T>
{
  private readonly Channel<T> _channel;

  public ChannelQueueService()
  {
    // Create an unbounded channel for simplicity
    _channel = Channel.CreateUnbounded<T>();
  }

  public async ValueTask EnqueueAsync(T item, CancellationToken cancellationToken = default)
  {
    await _channel.Writer.WriteAsync(item, cancellationToken);
  }

  public async ValueTask<T> DequeueAsync(CancellationToken cancellationToken = default)
  {
    return await _channel.Reader.ReadAsync(cancellationToken);
  }
}
