using MassTransit;
using Mediator;
using Microsoft.Extensions.Logging;
using Nimble.Modulith.Email.Contracts;

namespace Nimble.Modulith.Email.Integrations;

public class SendEmailConsumer : IConsumer<SendEmailCommand>
{
    private readonly IMediator _mediator;
    private readonly ILogger<SendEmailConsumer> _logger;

    public SendEmailConsumer(IMediator mediator, ILogger<SendEmailConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendEmailCommand> context)
    {
        _logger.LogInformation("Processing email request for {To}", context.Message.To);
        
        try
        {
            await _mediator.Send(context.Message, context.CancellationToken);
            _logger.LogInformation("Email processed successfully for {To}", context.Message.To);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process email for {To}", context.Message.To);
            throw;
        }
    }
}