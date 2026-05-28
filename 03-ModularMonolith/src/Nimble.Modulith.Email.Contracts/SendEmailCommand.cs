using Mediator;

namespace Nimble.Modulith.Email.Contracts;

public record SendEmailCommand(
  string To,
  string Subject,
  string Body,
  string? From = null) : ICommand;
