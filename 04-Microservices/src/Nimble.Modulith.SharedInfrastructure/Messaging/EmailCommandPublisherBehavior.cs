using MassTransit;
using Mediator;
using Microsoft.Extensions.Logging;
using Nimble.Modulith.Email.Contracts;

namespace Nimble.Modulith.SharedInfrastructure.Messaging;

/// <summary>
/// Intercepts SendEmailCommand and publishes it to RabbitMQ instead of handling it in-process.
/// This allows the Email module to be a separate microservice.
/// </summary>
public class EmailCommandPublisherBehavior : IPipelineBehavior<SendEmailCommand, Unit>
{
    private readonly IBus _bus;
    private readonly ILogger<EmailCommandPublisherBehavior> _logger;

    public EmailCommandPublisherBehavior(IBus bus, ILogger<EmailCommandPublisherBehavior> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async ValueTask<Unit> Handle(
        SendEmailCommand message, 
        MessageHandlerDelegate<SendEmailCommand, Unit> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Intercepting SendEmailCommand for {To} with subject '{Subject}' - publishing to message bus", 
            message.To, 
            message.Subject);

        // Publish to RabbitMQ instead of handling in-process
        await _bus.Publish(message, cancellationToken);

        _logger.LogInformation("SendEmailCommand published to message bus for {To}", message.To);

        // Don't call next - we're replacing the in-process handler with the message bus
        return Unit.Value;
    }
}