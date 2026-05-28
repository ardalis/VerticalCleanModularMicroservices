# Email Microservice Architecture

## Overview

The Email module has been carved out as a separate microservice from the modular monolith. This demonstrates the evolution from a modular monolith to a microservices architecture.

## Architecture Components

### 1. **Nimble.Modulith.Email** (Class Library)
- Contains the core email sending logic
- Services: `SmtpEmailSender`, `EmailSendingBackgroundWorker`
- No database dependencies (stateless)

### 2. **Nimble.Modulith.Email.Contracts** (Class Library)
- Contains the `SendEmailCommand` used for messaging
- Referenced by both the main web app and the email microservice
- Enables contract-based communication

### 3. **Nimble.Modulith.Email.Web** (Web Application)
- NEW: Standalone microservice web host
- Runs independently as a separate service
- Consumes `SendEmailCommand` messages from RabbitMQ via MassTransit
- Processes email sending requests asynchronously

### 4. **Nimble.Modulith.SharedInfrastructure** (Class Library)
- NEW: Shared infrastructure for cross-cutting concerns
- Contains `EmailCommandPublisherBehavior` - a Mediator pipeline behavior
- Intercepts `SendEmailCommand` and publishes to RabbitMQ instead of in-process handling
- Configured in the main web app via `AddSharedMessagingInfrastructure()`

## Communication Flow

```
┌─────────────────────────────────────────────────────────────────┐
│ Main Web App (Nimble.Modulith.Web)                             │
│                                                                 │
│  Module (e.g., Users)                                          │
│    └─> mediator.Send(SendEmailCommand)                        │
│                                                                 │
│  SharedInfrastructure                                          │
│    └─> EmailCommandPublisherBehavior (Pipeline Behavior)      │
│         └─> Intercepts command                                 │
│         └─> Publishes to RabbitMQ                             │
└─────────────────────────────────────────────────────────────────┘
                            │
                            │ MassTransit + RabbitMQ
                            ▼
┌─────────────────────────────────────────────────────────────────┐
│ Email Microservice (Nimble.Modulith.Email.Web)                 │
│                                                                 │
│  MassTransit Consumer                                          │
│    └─> SendEmailConsumer                                       │
│         └─> Receives command from RabbitMQ                     │
│         └─> mediator.Send() to Email module handler           │
│                                                                 │
│  Email Module                                                   │
│    └─> SendEmailCommandHandler                                │
│         └─> Processes email (SMTP)                             │
└─────────────────────────────────────────────────────────────────┘
```

## Key Benefits

1. **Zero Code Changes in Modules**: Modules continue to use `mediator.Send()` - they don't know the email is now out-of-process
2. **Clean Separation**: Main web app doesn't have direct MassTransit dependencies
3. **Infrastructure Abstraction**: SharedInfrastructure handles cross-cutting messaging concerns
4. **Independent Deployment**: Email service can be deployed, scaled, and updated independently
5. **Asynchronous Processing**: Email sending doesn't block the main application

## Aspire Configuration

The `AppHost` orchestrates both services:

```csharp
// RabbitMQ for messaging
var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin();

// Main web application
var webapi = builder.AddProject<Projects.Nimble_Modulith_Web>("webapi")
    .WithReference(rabbitmq)
    .WithReference(...databases...);

// Email microservice
var emailService = builder.AddProject<Projects.Nimble_Modulith_Email_Web>("email-service")
    .WithReference(rabbitmq)
    .WithEnvironment("Email__SmtpServer", "localhost");
```

## Configuration

### Main Web App (appsettings.json)
```json
{
  "ConnectionStrings": {
    "rabbitmq": "amqp://localhost"
  }
}
```

### Email Microservice (appsettings.json)
```json
{
  "Email": {
    "SmtpServer": "localhost",
    "SmtpPort": 25,
    "FromEmail": "noreply@nimblemodulith.com"
  },
  "ConnectionStrings": {
    "rabbitmq": "amqp://localhost"
  }
}
```

## Running the Application

1. Start the Aspire AppHost: `dotnet run --project Nimble.Modulith.AppHost`
2. This will automatically start:
   - RabbitMQ (message broker)
   - SQL Server (databases)
   - Papercut (SMTP test server)
   - Main Web API
   - Email Microservice

## Future Considerations

This pattern can be extended to other modules:
- Create a similar behavior for other cross-cutting concerns
- Extract more modules as microservices using the same pattern
- Add retry policies, circuit breakers, and other resilience patterns in SharedInfrastructure