using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nimble.Modulith.Email.Integrations;
using Nimble.Modulith.Email.Interfaces;
using Nimble.Modulith.Email.Services;

namespace Nimble.Modulith.Email;

public static class EmailModuleExtensions
{
    public static WebApplicationBuilder AddEmailModuleServices(
      this WebApplicationBuilder builder,
      Serilog.ILogger logger)
    {
        logger.Information("Adding Email module services...");

        // Configure email settings
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));

        // Register email sender as singleton (thread-safe, no scoped dependencies)
        builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();
        builder.Services.AddScoped<SendEmailCommandHandler>();

        // Register queue service as singleton (shared across all requests)
        builder.Services.AddSingleton(typeof(IQueueService<>), typeof(ChannelQueueService<>));

        // Register the background worker
        builder.Services.AddHostedService<EmailSendingBackgroundWorker>();

        logger.Information("Email module services added successfully");

        return builder;
    }
}
