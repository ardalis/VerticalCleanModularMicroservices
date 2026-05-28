namespace OrderDemo.CleanArch.Web.Configurations;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
  private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

  public async ValueTask<TResponse> Handle(TRequest message, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
  {
    var requestName = typeof(TRequest).Name;
    _logger.LogInformation("Handling {RequestName}", requestName);

    try
    {
      var response = await next(message, cancellationToken);
      _logger.LogInformation("Handled {RequestName} successfully", requestName);
      return response;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error handling {RequestName}", requestName);
      throw;
    }
  }
}
