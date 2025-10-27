using MassTransit;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nimble.Modulith.SharedInfrastructure.Messaging;

namespace Nimble.Modulith.SharedInfrastructure;

public static class SharedInfrastructureExtensions
{
    /// <summary>
    /// Adds MassTransit with RabbitMQ and registers messaging behaviors for inter-service communication.
    /// </summary>
    public static WebApplicationBuilder AddSharedMessagingInfrastructure(
        this WebApplicationBuilder builder)
    {
        // Add MassTransit with RabbitMQ
        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));
                cfg.ConfigureEndpoints(context);
            });
        });

        // Register the email command publisher behavior to intercept SendEmailCommand
        // and publish it to the message bus instead of handling it in-process
        builder.Services.AddScoped<IPipelineBehavior<Email.Contracts.SendEmailCommand, Unit>, 
            EmailCommandPublisherBehavior>();

        return builder;
    }
}