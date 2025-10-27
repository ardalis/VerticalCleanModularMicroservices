using Mediator;
using Serilog;

namespace Nimble.Modulith.Email.Web;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async ValueTask<TResponse> Handle(TRequest message, MessageHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        Log.Information("Handling {RequestName}", requestName);
        
        try
        {
            var response = await next(message, cancellationToken);
            Log.Information("Handled {RequestName} successfully", requestName);
            return response;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error handling {RequestName}", requestName);
            throw;
        }
    }
}